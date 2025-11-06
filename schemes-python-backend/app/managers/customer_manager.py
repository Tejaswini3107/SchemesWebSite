from sqlalchemy.orm import Session
from typing import List, Optional
from datetime import datetime
import requests
import asyncio
from app.repositories.customer_repository import CustomerRepository
from app.schemas import (
    SchemeDetailsCreate, SchemeDetailsUpdate, SchemeDetails,
    Customer, LoanDetailsCreate, LoanDetails,
    AdminCreate, EMICalculationRequest, EMICalculationResponse
)

class CustomerManager:
    def __init__(self, db: Session):
        self.db = db
        self.customer_repo = CustomerRepository(db)
    
    def get_schemes_list(self, name: str, lang_code: str = "en") -> List[dict]:
        """Get schemes list with optional language support"""
        try:
            if lang_code and lang_code != "en":
                return self.customer_repo.get_schemes_list_by_lang_code(name, lang_code)
            else:
                return self.customer_repo.get_schemes_list(name)
        except Exception as e:
            print(f"Get schemes list error: {e}")
            raise e
    
    def get_schemes_list_by_lang_code(self, name: str, lang_code: str) -> List[dict]:
        """Get schemes list by language code"""
        try:
            return self.customer_repo.get_schemes_list_by_lang_code(name, lang_code)
        except Exception as e:
            print(f"Get schemes list by lang code error: {e}")
            raise e
    
    def get_all_schemes_list(self) -> List[dict]:
        """Get all active schemes"""
        try:
            return self.customer_repo.get_all_schemes_list()
        except Exception as e:
            print(f"Get all schemes list error: {e}")
            raise e
    
    def add_new_scheme(self, scheme_details: SchemeDetailsCreate) -> bool:
        """Add new scheme with multilingual support"""
        try:
            scheme_id = self.customer_repo.add_new_scheme(scheme_details)
            
            # Prepare texts for translation
            texts_to_translate = [
                scheme_details.name_of_the_scheme,
                scheme_details.description,
                scheme_details.eligibility_criteria,
                scheme_details.benefits,
                scheme_details.area,
                scheme_details.documents_required,
                scheme_details.apply_and_link,
                scheme_details.available_for
            ]
            
            # Call translation API asynchronously
            asyncio.create_task(self.translation_api(texts_to_translate, scheme_id))
            
            return True
        except Exception as e:
            print(f"Add new scheme error: {e}")
            raise e
    
    def get_customers_list(self) -> List[Customer]:
        """Get all customers"""
        try:
            return self.customer_repo.get_customers_list()
        except Exception as e:
            print(f"Get customers list error: {e}")
            raise e
    
    def get_schemes_details(self, scheme_id: int) -> Optional[dict]:
        """Get specific scheme details"""
        try:
            return self.customer_repo.get_schemes_details(scheme_id)
        except Exception as e:
            print(f"Get schemes details error: {e}")
            raise e
    
    def update_scheme(self, scheme_id: int, scheme_details: SchemeDetailsUpdate) -> bool:
        """Update existing scheme"""
        try:
            return self.customer_repo.update_scheme(scheme_id, scheme_details)
        except Exception as e:
            print(f"Update scheme error: {e}")
            raise e
    
    def delete_scheme(self, scheme_id: int) -> bool:
        """Delete (deactivate) scheme"""
        try:
            return self.customer_repo.update_scheme_status(scheme_id)
        except Exception as e:
            print(f"Delete scheme error: {e}")
            raise e
    
    def update_customer_status(self, customer_id: int) -> bool:
        """Update customer status"""
        try:
            return self.customer_repo.update_customer_status(customer_id)
        except Exception as e:
            print(f"Update customer status error: {e}")
            raise e
    
    def get_loan_details(self, bank_name: str, loan_type: str) -> Optional[LoanDetails]:
        """Get loan details"""
        try:
            return self.customer_repo.get_loan_details(bank_name, loan_type)
        except Exception as e:
            print(f"Get loan details error: {e}")
            raise e
    
    def get_loan_types(self, bank_name: str) -> List[str]:
        """Get loan types for a bank"""
        try:
            return self.customer_repo.get_loan_types(bank_name)
        except Exception as e:
            print(f"Get loan types error: {e}")
            raise e
    
    def get_all_banks_list(self) -> List[LoanDetails]:
        """Get all loan details"""
        try:
            return self.customer_repo.get_loan_details_list()
        except Exception as e:
            print(f"Get all banks list error: {e}")
            raise e
    
    def add_banks_loan_details(self, loan_details: LoanDetailsCreate) -> bool:
        """Add new loan details"""
        try:
            return self.customer_repo.add_loan_details(loan_details)
        except Exception as e:
            print(f"Add banks loan details error: {e}")
            raise e
    
    def get_loan_details_by_id(self, loan_id: int) -> Optional[LoanDetails]:
        """Get loan details by ID"""
        try:
            loans = self.customer_repo.get_loan_details_list()
            for loan in loans:
                if loan.loan_interest_detail_id == loan_id:
                    return loan
            return None
        except Exception as e:
            print(f"Get loan details by ID error: {e}")
            raise e
    
    def update_loan_details(self, loan_id: int, loan_details: LoanDetailsCreate) -> bool:
        """Update loan details"""
        try:
            return self.customer_repo.update_loan_details(loan_id, loan_details)
        except Exception as e:
            print(f"Update loan details error: {e}")
            raise e
    
    def create_new_admin(self, admin_data: AdminCreate) -> bool:
        """Create new admin"""
        try:
            return self.customer_repo.create_new_admin(admin_data)
        except Exception as e:
            print(f"Create new admin error: {e}")
            raise e
    
    def calculate_emi(self, calculation_request: EMICalculationRequest) -> EMICalculationResponse:
        """Calculate EMI for loan"""
        try:
            principal = calculation_request.loan_amount
            annual_rate = calculation_request.interest_rate / 100
            monthly_rate = annual_rate / 12
            months = calculation_request.loan_tenure_months
            
            # EMI calculation formula
            if monthly_rate > 0:
                emi = principal * (monthly_rate * (1 + monthly_rate) ** months) / ((1 + monthly_rate) ** months - 1)
            else:
                emi = principal / months
            
            total_payment = emi * months
            total_interest = total_payment - principal
            
            return EMICalculationResponse(
                monthly_emi=round(emi, 2),
                total_interest=round(total_interest, 2),
                total_payment=round(total_payment, 2)
            )
        except Exception as e:
            print(f"Calculate EMI error: {e}")
            raise e
    
    async def translation_api(self, texts_to_translate: List[str], scheme_id: int):
        """Translate scheme details to multiple languages"""
        try:
            # Supported languages for translation
            target_languages = ["hi", "te", "ta", "bn", "mr", "gu"]  # Hindi, Telugu, Tamil, Bengali, Marathi, Gujarati
            
            for lang_code in target_languages:
                try:
                    # Call translation service (placeholder - integrate with Google Translate API, Azure Translator, etc.)
                    translated_texts = await self._translate_texts(texts_to_translate, lang_code)
                    
                    if translated_texts and len(translated_texts) >= 8:
                        # Create multilingual scheme record
                        from app.schemas import MultilingualSchemeCreate
                        multilingual_scheme = MultilingualSchemeCreate(
                            name_of_the_scheme=translated_texts[0],
                            description=translated_texts[1],
                            eligibility_criteria=translated_texts[2],
                            benefits=translated_texts[3],
                            area=translated_texts[4],
                            documents_required=translated_texts[5],
                            apply_and_link=translated_texts[6],
                            available_for=translated_texts[7],
                            lang_code=lang_code
                        )
                        
                        self.customer_repo.add_new_scheme_by_lang_code(
                            multilingual_scheme, scheme_id, lang_code
                        )
                        
                except Exception as lang_error:
                    print(f"Translation error for {lang_code}: {lang_error}")
                    continue
                    
        except Exception as e:
            print(f"Translation API error: {e}")
    
    async def _translate_texts(self, texts: List[str], target_language: str) -> List[str]:
        """Translate texts using external translation service"""
        # Placeholder for translation service integration
        # In production, integrate with Google Translate API, Azure Translator, etc.
        
        try:
            # Example using Google Translate API (you need to set up credentials)
            # from google.cloud import translate_v2 as translate
            # translate_client = translate.Client()
            # 
            # translated_texts = []
            # for text in texts:
            #     if text:
            #         result = translate_client.translate(text, target_language=target_language)
            #         translated_texts.append(result['translatedText'])
            #     else:
            #         translated_texts.append(text)
            # 
            # return translated_texts
            
            # For now, return original texts (placeholder)
            print(f"Translating {len(texts)} texts to {target_language}")
            return texts
            
        except Exception as e:
            print(f"Translation service error: {e}")
            return texts
