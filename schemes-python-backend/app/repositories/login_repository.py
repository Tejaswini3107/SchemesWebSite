from sqlalchemy.orm import Session
from sqlalchemy import and_, or_
from typing import Optional, List
from datetime import datetime, timedelta
import hashlib
import secrets
from app.models import Customer, CustomerLogin, OTPDetails
from app.schemas import CustomerCreate, RegistrationRequest, OTPTypeEnum

class LoginRepository:
    def __init__(self, db: Session):
        self.db = db
    
    def get_customer_login(self, email: str, password: str) -> Optional[int]:
        """Authenticate customer login and return customer ID"""
        hashed_password = self._hash_password(password)
        customer_login = self.db.query(CustomerLogin).filter(
            and_(
                CustomerLogin.email_id == email,
                CustomerLogin.password == hashed_password
            )
        ).first()
        
        if customer_login:
            # Update login status
            customer_login.login_status = True
            customer_login.updated_by = "System"
            customer_login.updated_date = datetime.utcnow()
            self.db.commit()
            return customer_login.customer_id
        return None
    
    def get_admin_login(self, username: str, password: str) -> Optional[int]:
        """Authenticate admin login and return admin ID"""
        from app.models import AdminLogin
        hashed_password = self._hash_password(password)
        admin_login = self.db.query(AdminLogin).filter(
            and_(
                AdminLogin.user_name == username,
                AdminLogin.password == hashed_password
            )
        ).first()
        
        if admin_login:
            # Update login status
            admin_login.login_status = True
            admin_login.updated_by = "System"
            admin_login.updated_date = datetime.utcnow()
            self.db.commit()
            return admin_login.admin_id
        return None
    
    def add_customer_registration_details(self, registration: RegistrationRequest) -> int:
        """Add customer details during registration"""
        customer = Customer(
            first_name=registration.first_name,
            last_name=registration.last_name,
            email_id=registration.email_id,
            phone_no=registration.phone_no,
            date_of_birth=registration.date_of_birth,
            customer_status=10,  # Active
            inserted_by="System",
            inserted_date=datetime.utcnow()
        )
        
        self.db.add(customer)
        self.db.commit()
        self.db.refresh(customer)
        return customer.customer_id
    
    def add_customer_registration_login_details(self, registration: RegistrationRequest, customer_id: int):
        """Add customer login details during registration"""
        hashed_password = self._hash_password(registration.password)
        
        customer_login = CustomerLogin(
            customer_id=customer_id,
            email_id=registration.email_id,
            password=hashed_password,
            login_status=False,
            inserted_by="System",
            inserted_date=datetime.utcnow()
        )
        
        self.db.add(customer_login)
        self.db.commit()
    
    def add_otp(self, email_id: Optional[str] = None, otp_type: OTPTypeEnum = OTPTypeEnum.REGISTRATION_EMAIL, 
                phone_number: Optional[str] = None):
        """Generate and store OTP"""
        otp = self._generate_otp()
        expiry_time = datetime.utcnow() + timedelta(minutes=10)  # 10 minutes expiry
        
        otp_details = OTPDetails(
            email_id=email_id,
            phone_number=phone_number,
            otp=otp,
            otp_type=otp_type,
            otp_status=0,  # Sent
            expiry_time=expiry_time,
            inserted_by="System",
            inserted_date=datetime.utcnow()
        )
        
        self.db.add(otp_details)
        self.db.commit()
        
        # Here you would integrate with email/SMS service
        if email_id:
            self._send_email_otp(email_id, otp)
        if phone_number:
            self._send_sms_otp(phone_number, otp)
    
    def verify_otp(self, email_or_phone: str, otp: str) -> bool:
        """Verify OTP"""
        otp_record = self.db.query(OTPDetails).filter(
            and_(
                or_(
                    OTPDetails.email_id == email_or_phone,
                    OTPDetails.phone_number == email_or_phone
                ),
                OTPDetails.otp == otp,
                OTPDetails.expiry_time > datetime.utcnow(),
                OTPDetails.otp_status == 0  # Sent status
            )
        ).first()
        
        if otp_record:
            otp_record.otp_status = 2  # Success
            otp_record.updated_by = "System"
            otp_record.updated_date = datetime.utcnow()
            self.db.commit()
            return True
        return False
    
    def check_email_for_password_change(self, email_id: str) -> bool:
        """Check if email exists for password change"""
        customer = self.db.query(Customer).filter(Customer.email_id == email_id).first()
        return customer is not None
    
    def update_password(self, email_id: str, new_password: str):
        """Update customer password"""
        hashed_password = self._hash_password(new_password)
        customer_login = self.db.query(CustomerLogin).filter(
            CustomerLogin.email_id == email_id
        ).first()
        
        if customer_login:
            customer_login.password = hashed_password
            customer_login.updated_by = "System"
            customer_login.updated_date = datetime.utcnow()
            self.db.commit()
    
    def send_email_to_user(self, email: str, message: str, is_html: bool = False):
        """Send email to user - placeholder for email service integration"""
        # Integration with email service (SendGrid, AWS SES, etc.)
        print(f"Sending email to {email}: {message}")
    
    def send_otp_async(self, sms_details: List[dict]):
        """Send SMS notifications - placeholder for SMS service integration"""
        # Integration with SMS service (Twilio, AWS SNS, etc.)
        for sms in sms_details:
            print(f"Sending SMS to {sms['phone_number']}: {sms['text_message']}")
    
    def _hash_password(self, password: str) -> str:
        """Hash password using SHA-256 (in production, use bcrypt or similar)"""
        return hashlib.sha256(password.encode()).hexdigest()
    
    def _generate_otp(self) -> str:
        """Generate 6-digit OTP"""
        return str(secrets.randbelow(900000) + 100000)
    
    def _send_email_otp(self, email: str, otp: str):
        """Send OTP via email"""
        message = f"Your OTP for registration is: {otp}. Valid for 10 minutes."
        self.send_email_to_user(email, message)
    
    def _send_sms_otp(self, phone: str, otp: str):
        """Send OTP via SMS"""
        message = f"Your OTP for registration is: {otp}. Valid for 10 minutes."
        self.send_otp_async([{"phone_number": phone, "text_message": message}])
