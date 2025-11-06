from typing import Optional
from datetime import datetime
from sqlalchemy import Column, Integer, String, DateTime, Boolean, Text, ForeignKey
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import relationship

Base = declarative_base()

class BaseModel(Base):
    __abstract__ = True
    
    inserted_by = Column(String(255), nullable=False)
    inserted_date = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_by = Column(String(255), nullable=True)
    updated_date = Column(DateTime, nullable=True)

class Customer(BaseModel):
    __tablename__ = "customers"
    
    customer_id = Column(Integer, primary_key=True, index=True)
    first_name = Column(String(255), nullable=True)
    last_name = Column(String(255), nullable=True)
    email_id = Column(String(255), nullable=True, unique=True, index=True)
    phone_no = Column(String(20), nullable=True)
    date_of_birth = Column(DateTime, nullable=True)
    customer_status = Column(Integer, default=10, nullable=False)  # 10=Active, 20=Inactive
    
    # Relationship
    customer_login = relationship("CustomerLogin", back_populates="customer", uselist=False)

class CustomerLogin(BaseModel):
    __tablename__ = "customer_login"
    
    customer_login_id = Column(Integer, primary_key=True, index=True)
    customer_id = Column(Integer, ForeignKey("customers.customer_id"), nullable=False)
    email_id = Column(String(255), nullable=True)
    password = Column(String(255), nullable=False)
    login_status = Column(Boolean, default=False)
    
    # Relationship
    customer = relationship("Customer", back_populates="customer_login")

class Admin(BaseModel):
    __tablename__ = "admin"
    
    admin_id = Column(Integer, primary_key=True, index=True)
    first_name = Column(String(255), nullable=True)
    last_name = Column(String(255), nullable=True)
    email_id = Column(String(255), nullable=True, unique=True, index=True)
    phone_no = Column(String(20), nullable=True)
    
    # Relationship
    admin_login = relationship("AdminLogin", back_populates="admin", uselist=False)

class AdminLogin(BaseModel):
    __tablename__ = "admin_login"
    
    admin_login_id = Column(Integer, primary_key=True, index=True)
    admin_id = Column(Integer, ForeignKey("admin.admin_id"), nullable=False)
    email_id = Column(String(255), nullable=True)
    user_name = Column(String(255), nullable=False, unique=True)
    password = Column(String(255), nullable=False)
    login_status = Column(Boolean, default=False)
    
    # Relationship
    admin = relationship("Admin", back_populates="admin_login")

class SchemesDetails(BaseModel):
    __tablename__ = "schemes_details"
    
    schemes_detail_id = Column(Integer, primary_key=True, index=True)
    available_for = Column(String(500), nullable=True)
    name_of_the_scheme = Column(String(500), nullable=True)
    description = Column(Text, nullable=True)
    eligibility_criteria = Column(Text, nullable=True)
    benefits = Column(Text, nullable=True)
    area = Column(String(255), nullable=True)
    documents_required = Column(Text, nullable=True)
    apply_and_link = Column(Text, nullable=True)
    is_active = Column(Boolean, default=True, nullable=False)
    
    # Relationship
    multilingual_data = relationship("MultilingualSchemesData", back_populates="scheme")

class MultilingualSchemesData(BaseModel):
    __tablename__ = "multilingual_schemes_data"
    
    multilingual_schemes_data_id = Column(Integer, primary_key=True, index=True)
    schemes_detail_id = Column(Integer, ForeignKey("schemes_details.schemes_detail_id"), nullable=False)
    lang_code = Column(String(10), nullable=False)
    available_for = Column(String(500), nullable=True)
    name_of_the_scheme = Column(String(500), nullable=True)
    description = Column(Text, nullable=True)
    eligibility_criteria = Column(Text, nullable=True)
    benefits = Column(Text, nullable=True)
    area = Column(String(255), nullable=True)
    documents_required = Column(Text, nullable=True)
    apply_and_link = Column(Text, nullable=True)
    is_active = Column(Boolean, default=True, nullable=False)
    
    # Relationship
    scheme = relationship("SchemesDetails", back_populates="multilingual_data")

class LoanInterestDetails(BaseModel):
    __tablename__ = "loan_interest_details"
    
    loan_interest_detail_id = Column(Integer, primary_key=True, index=True)
    bank_name = Column(String(255), nullable=False)
    loan_type = Column(String(255), nullable=False)
    min_loan_amount = Column(String(50), nullable=False)
    max_loan_amount = Column(String(50), nullable=False)
    min_interest_rate = Column(String(10), nullable=False)
    max_interest_rate = Column(String(10), nullable=False)

class OTPDetails(BaseModel):
    __tablename__ = "otp_details"
    
    otp_details_id = Column(Integer, primary_key=True, index=True)
    email_id = Column(String(255), nullable=True)
    phone_number = Column(String(20), nullable=True)
    otp = Column(String(10), nullable=False)
    otp_type = Column(Integer, nullable=False)  # 0=Email Registration, 1=Phone Registration, 2=Forgot Password
    otp_status = Column(Integer, default=0, nullable=False)  # 0=Sent, 1=Failed, 2=Success
    expiry_time = Column(DateTime, nullable=False)
