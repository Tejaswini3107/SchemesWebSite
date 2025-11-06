from pydantic import BaseModel, EmailStr
from typing import Optional, List
from datetime import datetime
from enum import IntEnum

class OTPTypeEnum(IntEnum):
    REGISTRATION_EMAIL = 0
    REGISTRATION_PHONE = 1
    FORGOT_PASSWORD = 2

class OTPStatusEnum(IntEnum):
    SENT = 0
    FAILED = 1
    SUCCESS = 2

class CustomerStatusEnum(IntEnum):
    ACTIVE = 10
    INACTIVE = 20

# Base schemas
class CustomerBase(BaseModel):
    first_name: Optional[str] = None
    last_name: Optional[str] = None
    email_id: Optional[EmailStr] = None
    phone_no: Optional[str] = None
    date_of_birth: Optional[datetime] = None

class CustomerCreate(CustomerBase):
    password: str

class CustomerUpdate(CustomerBase):
    customer_status: Optional[int] = None

class Customer(CustomerBase):
    customer_id: int
    customer_status: int
    inserted_by: str
    inserted_date: datetime
    updated_by: Optional[str] = None
    updated_date: Optional[datetime] = None
    
    class Config:
        from_attributes = True

# Login schemas
class CustomerLoginRequest(BaseModel):
    email: str
    password: str

class AdminLoginRequest(BaseModel):
    username: str
    password: str

class RegistrationRequest(BaseModel):
    first_name: str
    last_name: str
    email_id: EmailStr
    phone_no: str
    password: str
    date_of_birth: Optional[datetime] = None

class ForgotPasswordRequest(BaseModel):
    email_id: EmailStr

class PasswordUpdateRequest(BaseModel):
    email_id: EmailStr
    new_password: str
    otp: str

# OTP schemas
class OTPRequest(BaseModel):
    email_id: Optional[EmailStr] = None
    phone_number: Optional[str] = None
    otp_type: OTPTypeEnum

class OTPVerifyRequest(BaseModel):
    email_or_phone: str
    otp: str

# Scheme schemas
class SchemeDetailsBase(BaseModel):
    name_of_the_scheme: Optional[str] = None
    description: Optional[str] = None
    eligibility_criteria: Optional[str] = None
    benefits: Optional[str] = None
    area: Optional[str] = None
    documents_required: Optional[str] = None
    apply_and_link: Optional[str] = None
    available_for: Optional[str] = None

class SchemeDetailsCreate(SchemeDetailsBase):
    pass

class SchemeDetailsUpdate(SchemeDetailsBase):
    is_active: Optional[bool] = None

class SchemeDetails(SchemeDetailsBase):
    schemes_detail_id: int
    is_active: bool
    inserted_by: str
    inserted_date: datetime
    updated_by: Optional[str] = None
    updated_date: Optional[datetime] = None
    
    class Config:
        from_attributes = True

# Multilingual schema
class MultilingualSchemeCreate(SchemeDetailsBase):
    lang_code: str

class MultilingualScheme(SchemeDetailsBase):
    multilingual_schemes_data_id: int
    schemes_detail_id: int
    lang_code: str
    is_active: bool
    
    class Config:
        from_attributes = True

# Loan schemas
class LoanDetailsBase(BaseModel):
    bank_name: str
    loan_type: str
    min_loan_amount: str
    max_loan_amount: str
    min_interest_rate: str
    max_interest_rate: str

class LoanDetailsCreate(LoanDetailsBase):
    pass

class LoanDetailsUpdate(LoanDetailsBase):
    pass

class LoanDetails(LoanDetailsBase):
    loan_interest_detail_id: int
    inserted_by: str
    inserted_date: datetime
    updated_by: Optional[str] = None
    updated_date: Optional[datetime] = None
    
    class Config:
        from_attributes = True

# Admin schemas
class AdminBase(BaseModel):
    first_name: Optional[str] = None
    last_name: Optional[str] = None
    email_id: Optional[EmailStr] = None
    phone_no: Optional[str] = None

class AdminCreate(AdminBase):
    username: str
    password: str

class Admin(AdminBase):
    admin_id: int
    inserted_by: str
    inserted_date: datetime
    
    class Config:
        from_attributes = True

class AdminWithCustomers(Admin):
    customer_details: List[Customer] = []

# Response schemas
class LoginResponse(BaseModel):
    user_id: int
    access_token: str
    token_type: str = "bearer"
    user_type: str  # "customer" or "admin"

class MessageResponse(BaseModel):
    message: str
    success: bool = True

class EMICalculationRequest(BaseModel):
    loan_amount: float
    interest_rate: float
    loan_tenure_months: int

class EMICalculationResponse(BaseModel):
    monthly_emi: float
    total_interest: float
    total_payment: float

# Scheme labels for multilingual support
class SchemesCustomerLabels(BaseModel):
    sidebar_label1: str
    sidebar_label2: str
    sidebar_label3: str
    sidebar_label4: str
    sidebar_label5: str
    emi_title: str
    banks_title: str
    bank1: str
    bank2: str
    bank3: str
    loan_amount_lbl: str
    interest_rate_lbl: str
    loan_tenure_lbl: str
    calculate_emi_btn_txt: str
    result_txt: str
    loan_emi_txt: str
    tip_txt: str
    tp_txt: str
    scheme_detail_list: Optional[List[SchemeDetails]] = None
