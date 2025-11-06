from sqlalchemy.orm import Session
from typing import Optional
from datetime import datetime
from app.repositories.login_repository import LoginRepository
from app.schemas import (
    CustomerLoginRequest, AdminLoginRequest, RegistrationRequest,
    OTPRequest, OTPVerifyRequest, ForgotPasswordRequest, PasswordUpdateRequest,
    OTPTypeEnum
)

class LoginManager:
    def __init__(self, db: Session):
        self.db = db
        self.login_repo = LoginRepository(db)
    
    def get_customer_login(self, login_request: CustomerLoginRequest) -> Optional[int]:
        """Authenticate customer login"""
        try:
            customer_id = self.login_repo.get_customer_login(
                login_request.email, 
                login_request.password
            )
            return customer_id
        except Exception as e:
            print(f"Customer login error: {e}")
            raise e
    
    def get_admin_login(self, login_request: AdminLoginRequest) -> Optional[int]:
        """Authenticate admin login"""
        try:
            admin_id = self.login_repo.get_admin_login(
                login_request.username,
                login_request.password
            )
            return admin_id
        except Exception as e:
            print(f"Admin login error: {e}")
            raise e
    
    def customer_registration(self, registration: RegistrationRequest) -> bool:
        """Register new customer"""
        try:
            # Add customer details
            customer_id = self.login_repo.add_customer_registration_details(registration)
            
            # Add customer login details
            self.login_repo.add_customer_registration_login_details(registration, customer_id)
            
            return True
        except Exception as e:
            print(f"Customer registration error: {e}")
            raise e
    
    def send_otp(self, otp_request: OTPRequest) -> bool:
        """Send OTP for various purposes"""
        try:
            self.login_repo.add_otp(
                email_id=otp_request.email_id,
                otp_type=otp_request.otp_type,
                phone_number=otp_request.phone_number
            )
            return True
        except Exception as e:
            print(f"Send OTP error: {e}")
            raise e
    
    def send_otp_to_phone_number(self, phone_number: str, otp_type: OTPTypeEnum) -> bool:
        """Send OTP to phone number"""
        try:
            self.login_repo.add_otp(
                email_id=None,
                otp_type=otp_type,
                phone_number=phone_number
            )
            return True
        except Exception as e:
            print(f"Send OTP to phone error: {e}")
            raise e
    
    def verify_otp(self, verify_request: OTPVerifyRequest) -> bool:
        """Verify OTP"""
        try:
            is_verified = self.login_repo.verify_otp(
                verify_request.email_or_phone,
                verify_request.otp
            )
            return is_verified
        except Exception as e:
            print(f"Verify OTP error: {e}")
            raise e
    
    def check_email_for_password_change(self, email_id: str) -> bool:
        """Check if email exists for password reset"""
        try:
            return self.login_repo.check_email_for_password_change(email_id)
        except Exception as e:
            print(f"Check email error: {e}")
            raise e
    
    def update_password(self, password_request: PasswordUpdateRequest) -> bool:
        """Update password after OTP verification"""
        try:
            # First verify OTP
            is_otp_valid = self.login_repo.verify_otp(
                password_request.email_id,
                password_request.otp
            )
            
            if is_otp_valid:
                # Update password
                self.login_repo.update_password(
                    password_request.email_id,
                    password_request.new_password
                )
                return True
            return False
        except Exception as e:
            print(f"Update password error: {e}")
            raise e
    
    def forgot_password(self, forgot_request: ForgotPasswordRequest) -> bool:
        """Initiate forgot password process"""
        try:
            # Check if email exists
            email_exists = self.check_email_for_password_change(forgot_request.email_id)
            
            if email_exists:
                # Send OTP for password reset
                otp_request = OTPRequest(
                    email_id=forgot_request.email_id,
                    otp_type=OTPTypeEnum.FORGOT_PASSWORD
                )
                return self.send_otp(otp_request)
            return False
        except Exception as e:
            print(f"Forgot password error: {e}")
            raise e
