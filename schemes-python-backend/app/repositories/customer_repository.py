from sqlalchemy.orm import Session
from sqlalchemy import and_, desc
from typing import List, Optional
from datetime import datetime
import re
from app.models import (
    SchemesDetails, MultilingualSchemesData, Customer, 
    LoanInterestDetails, Admin, AdminLogin
)
from app.schemas import (
    SchemeDetailsCreate, SchemeDetailsUpdate, Customer as CustomerSchema,
    LoanDetailsCreate, LoanDetails as LoanDetailsSchema,
    AdminCreate
)

class CustomerRepository:
    def __init__(self, db: Session):
        self.db = db
    
    def get_schemes_list(self, name: str) -> List[dict]:
        """Get schemes list filtered by available_for containing name"""
        schemes = self.db.query(SchemesDetails).filter(
            and_(
                SchemesDetails.available_for.contains(name),
                SchemesDetails.is_active == True
            )
        ).order_by(desc(SchemesDetails.inserted_date)).all()
        
        return self._convert_schemes_to_dict(schemes)
    
    def get_schemes_list_by_lang_code(self, name: str, lang_code: str) -> List[dict]:
        """Get schemes list by language code"""
        multilingual_schemes = self.db.query(MultilingualSchemesData).filter(
            and_(
                MultilingualSchemesData.available_for.contains(name),
                MultilingualSchemesData.lang_code == lang_code,
                MultilingualSchemesData.is_active == True
            )
        ).all()
        
        if multilingual_schemes:
            result = []
            for scheme in multilingual_schemes:
                scheme_dict = {
                    "schemes_detail_id": scheme.schemes_detail_id,
                    "area": scheme.area,
                    "apply_and_link": scheme.apply_and_link,
                    "name_of_the_scheme": scheme.name_of_the_scheme,
                    "benefits": scheme.benefits,
                    "eligibility_criteria": scheme.eligibility_criteria,
                    "description": scheme.description,
                    "documents_required": scheme.documents_required,
                    "available_for": scheme.available_for,
                    "lang": scheme.lang_code
                }
                result.append(scheme_dict)
            return result
        else:
            # Fallback to default schemes
            return self.get_schemes_list(name)
    
    def get_all_schemes_list(self) -> List[dict]:
        """Get all active schemes"""
        schemes = self.db.query(SchemesDetails).filter(
            SchemesDetails.is_active == True
        ).order_by(desc(SchemesDetails.inserted_date)).all()
        
        return self._convert_schemes_to_dict(schemes)
    
    def add_new_scheme(self, scheme: SchemeDetailsCreate) -> int:
        """Add new scheme and return scheme ID"""
        scheme_details = SchemesDetails(
            area=scheme.area,
            apply_and_link=self._replace_urls_with_links(scheme.apply_and_link) if scheme.apply_and_link else None,
            name_of_the_scheme=scheme.name_of_the_scheme,
            benefits=scheme.benefits,
            eligibility_criteria=scheme.eligibility_criteria,
            description=scheme.description,
            documents_required=scheme.documents_required,
            available_for=scheme.available_for,
            is_active=True,
            inserted_date=datetime.utcnow(),
            inserted_by="Admin"
        )
        
        self.db.add(scheme_details)
        self.db.commit()
        self.db.refresh(scheme_details)
        
        # Add multilingual data for English
        self.add_new_scheme_by_lang_code(scheme, scheme_details.schemes_detail_id, "en")
        
        return scheme_details.schemes_detail_id
    
    def add_new_scheme_by_lang_code(self, scheme: SchemeDetailsCreate, scheme_id: int, lang_code: str) -> bool:
        """Add multilingual scheme data"""
        multilingual_scheme = MultilingualSchemesData(
            area=scheme.area,
            apply_and_link=self._replace_urls_with_links(scheme.apply_and_link) if scheme.apply_and_link else None,
            name_of_the_scheme=scheme.name_of_the_scheme,
            benefits=scheme.benefits,
            eligibility_criteria=scheme.eligibility_criteria,
            description=scheme.description,
            documents_required=scheme.documents_required,
            available_for=scheme.available_for,
            lang_code=lang_code,
            schemes_detail_id=scheme_id,
            is_active=True,
            inserted_date=datetime.utcnow(),
            inserted_by="Admin"
        )
        
        self.db.add(multilingual_scheme)
        self.db.commit()
        return True
    
    def update_scheme(self, scheme_id: int, scheme_update: SchemeDetailsUpdate) -> bool:
        """Update existing scheme"""
        scheme = self.db.query(SchemesDetails).filter(
            SchemesDetails.schemes_detail_id == scheme_id
        ).first()
        
        if scheme:
            if scheme_update.area is not None:
                scheme.area = scheme_update.area
            if scheme_update.apply_and_link is not None:
                scheme.apply_and_link = self._replace_urls_with_links(scheme_update.apply_and_link)
            if scheme_update.name_of_the_scheme is not None:
                scheme.name_of_the_scheme = scheme_update.name_of_the_scheme
            if scheme_update.benefits is not None:
                scheme.benefits = scheme_update.benefits
            if scheme_update.eligibility_criteria is not None:
                scheme.eligibility_criteria = scheme_update.eligibility_criteria
            if scheme_update.description is not None:
                scheme.description = scheme_update.description
            if scheme_update.documents_required is not None:
                scheme.documents_required = scheme_update.documents_required
            if scheme_update.available_for is not None:
                scheme.available_for = scheme_update.available_for
            if scheme_update.is_active is not None:
                scheme.is_active = scheme_update.is_active
            
            scheme.updated_date = datetime.utcnow()
            scheme.updated_by = "Admin"
            self.db.commit()
            return True
        return False
    
    def update_scheme_status(self, scheme_id: int) -> bool:
        """Deactivate scheme (soft delete)"""
        scheme = self.db.query(SchemesDetails).filter(
            SchemesDetails.schemes_detail_id == scheme_id
        ).first()
        
        if scheme:
            scheme.is_active = False
            scheme.updated_date = datetime.utcnow()
            scheme.updated_by = "Admin"
            self.db.commit()
            return True
        return False
    
    def get_customers_list(self) -> List[CustomerSchema]:
        """Get all customers"""
        customers = self.db.query(Customer).all()
        return [CustomerSchema.from_orm(customer) for customer in customers]
    
    def update_customer_status(self, customer_id: int) -> bool:
        """Toggle customer status"""
        customer = self.db.query(Customer).filter(
            Customer.customer_id == customer_id
        ).first()
        
        if customer:
            customer.customer_status = 20 if customer.customer_status == 10 else 10  # Toggle Active/Inactive
            customer.updated_date = datetime.utcnow()
            customer.updated_by = "Admin"
            self.db.commit()
            return True
        return False
    
    def get_schemes_details(self, scheme_id: int) -> Optional[dict]:
        """Get specific scheme details"""
        scheme = self.db.query(SchemesDetails).filter(
            SchemesDetails.schemes_detail_id == scheme_id
        ).first()
        
        if scheme:
            return {
                "schemes_detail_id": scheme.schemes_detail_id,
                "area": scheme.area,
                "apply_and_link": scheme.apply_and_link,
                "name_of_the_scheme": scheme.name_of_the_scheme,
                "benefits": scheme.benefits,
                "eligibility_criteria": scheme.eligibility_criteria,
                "description": scheme.description,
                "documents_required": scheme.documents_required,
                "available_for": scheme.available_for
            }
        return None
    
    def get_loan_details(self, bank_name: str, loan_type: str) -> Optional[LoanDetailsSchema]:
        """Get loan details by bank name and loan type"""
        loan = self.db.query(LoanInterestDetails).filter(
            and_(
                LoanInterestDetails.bank_name.contains(bank_name),
                LoanInterestDetails.loan_type == loan_type
            )
        ).first()
        
        if loan:
            return LoanDetailsSchema.from_orm(loan)
        return None
    
    def get_loan_types(self, bank_name: str) -> List[str]:
        """Get loan types for a specific bank"""
        loan_types = self.db.query(LoanInterestDetails.loan_type).filter(
            LoanInterestDetails.bank_name.contains(bank_name)
        ).distinct().all()
        
        return [loan_type[0] for loan_type in loan_types]
    
    def get_loan_details_list(self) -> List[LoanDetailsSchema]:
        """Get all loan details"""
        loans = self.db.query(LoanInterestDetails).all()
        return [LoanDetailsSchema.from_orm(loan) for loan in loans]
    
    def add_loan_details(self, loan_details: LoanDetailsCreate) -> bool:
        """Add new loan details"""
        loan = LoanInterestDetails(
            bank_name=loan_details.bank_name,
            loan_type=loan_details.loan_type,
            min_loan_amount=loan_details.min_loan_amount,
            max_loan_amount=loan_details.max_loan_amount,
            min_interest_rate=loan_details.min_interest_rate,
            max_interest_rate=loan_details.max_interest_rate,
            inserted_by="Admin",
            inserted_date=datetime.utcnow()
        )
        
        self.db.add(loan)
        self.db.commit()
        
        # Send notifications to customers (placeholder)
        customers = self.get_customers_list()
        message = f"New loan offer: {loan_details.bank_name} bank is providing {loan_details.loan_type} loans at interest rates starting from {loan_details.min_interest_rate}%"
        
        # Here you would integrate with notification service
        print(f"Sending notifications to {len(customers)} customers: {message}")
        
        return True
    
    def update_loan_details(self, loan_id: int, loan_details: LoanDetailsCreate) -> bool:
        """Update existing loan details"""
        loan = self.db.query(LoanInterestDetails).filter(
            LoanInterestDetails.loan_interest_detail_id == loan_id
        ).first()
        
        if loan:
            loan.bank_name = loan_details.bank_name
            loan.loan_type = loan_details.loan_type
            loan.min_loan_amount = loan_details.min_loan_amount
            loan.max_loan_amount = loan_details.max_loan_amount
            loan.min_interest_rate = loan_details.min_interest_rate
            loan.max_interest_rate = loan_details.max_interest_rate
            loan.updated_date = datetime.utcnow()
            loan.updated_by = "Admin"
            self.db.commit()
            return True
        return False
    
    def create_new_admin(self, admin_data: AdminCreate) -> bool:
        """Create new admin"""
        # Create Admin record
        admin = Admin(
            first_name=admin_data.first_name,
            last_name=admin_data.last_name,
            email_id=admin_data.email_id,
            phone_no=admin_data.phone_no,
            inserted_by="SuperAdmin",
            inserted_date=datetime.utcnow()
        )
        
        self.db.add(admin)
        self.db.commit()
        self.db.refresh(admin)
        
        # Create AdminLogin record
        from repositories.login_repository import LoginRepository
        login_repo = LoginRepository(self.db)
        hashed_password = login_repo._hash_password(admin_data.password)
        
        admin_login = AdminLogin(
            admin_id=admin.admin_id,
            email_id=admin_data.email_id,
            user_name=admin_data.username,
            password=hashed_password,
            login_status=False,
            inserted_by="SuperAdmin",
            inserted_date=datetime.utcnow()
        )
        
        self.db.add(admin_login)
        self.db.commit()
        return True
    
    def _convert_schemes_to_dict(self, schemes: List[SchemesDetails]) -> List[dict]:
        """Convert SQLAlchemy models to dictionaries"""
        result = []
        for scheme in schemes:
            scheme_dict = {
                "schemes_detail_id": scheme.schemes_detail_id,
                "area": scheme.area,
                "apply_and_link": scheme.apply_and_link,
                "name_of_the_scheme": scheme.name_of_the_scheme,
                "benefits": scheme.benefits,
                "eligibility_criteria": scheme.eligibility_criteria,
                "description": scheme.description,
                "documents_required": scheme.documents_required,
                "available_for": scheme.available_for,
                "is_active": scheme.is_active
            }
            result.append(scheme_dict)
        return result
    
    def _replace_urls_with_links(self, text: str) -> str:
        """Replace URLs with HTML links"""
        if not text:
            return text
        
        url_pattern = r'(?<=(^|\s))(https?://\S+)'
        
        def replace_url(match):
            url = match.group(0)
            return f'<a class="btn btn-link" href="{url}">{url}</a>'
        
        return re.sub(url_pattern, replace_url, text)
