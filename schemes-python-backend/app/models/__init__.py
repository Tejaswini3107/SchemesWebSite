from typing import Optional
from datetime import datetime
from sqlalchemy import Column, Integer, String, DateTime, Boolean, Text, ForeignKey
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import relationship

Base = declarative_base()

class BaseModel(Base):
    __abstract__ = True

    inserted_by = Column(String(100), nullable=False)
    inserted_date = Column(DateTime, default=datetime.utcnow, nullable=False)
    updated_by = Column(String(100), nullable=True)
    updated_date = Column(DateTime, nullable=True)

class Customer(BaseModel):
    __tablename__ = "customers"

    customer_id = Column(Integer, primary_key=True, autoincrement=True)
    first_name = Column(String(100), nullable=True)
    last_name = Column(String(100), nullable=True)
    email_id = Column(String(255), nullable=True)
    phone_no = Column(String(20), nullable=True)
    date_of_birth = Column(DateTime, nullable=True)
    customer_status = Column(Integer, default=1, nullable=False)

    # Relationship
    customer_login = relationship("CustomerLogin", back_populates="customer", uselist=False)

class CustomerLogin(BaseModel):
    __tablename__ = "customer_logins"

    id = Column(Integer, primary_key=True, autoincrement=True)
    customer_id = Column(Integer, ForeignKey('customers.customer_id'), nullable=False)
    username = Column(String(100), unique=True, nullable=False)
    password_hash = Column(String(255), nullable=False)
    login_attempts = Column(Integer, default=0, nullable=False)
    last_login = Column(DateTime, nullable=True)
    is_locked = Column(Boolean, default=False, nullable=False)

    # Relationship
    customer = relationship("Customer", backref="login")

class Admin(BaseModel):
    __tablename__ = "admins"

    admin_id = Column(Integer, primary_key=True, autoincrement=True)
    first_name = Column(String(100), nullable=True)
    last_name = Column(String(100), nullable=True)
    email_id = Column(String(255), nullable=True)
    phone_no = Column(String(20), nullable=True)

    # Relationship
    admin_login = relationship("AdminLogin", back_populates="admin", uselist=False)

class AdminLogin(BaseModel):
    __tablename__ = "admin_logins"

    id = Column(Integer, primary_key=True, autoincrement=True)
    admin_id = Column(Integer, ForeignKey('admins.admin_id'), nullable=False)
    username = Column(String(100), unique=True, nullable=False)
    password_hash = Column(String(255), nullable=False)
    login_attempts = Column(Integer, default=0, nullable=False)
    last_login = Column(DateTime, nullable=True)
    is_locked = Column(Boolean, default=False, nullable=False)

    # Relationship
    admin = relationship("Admin", backref="login")

class SchemesDetails(BaseModel):
    __tablename__ = "schemes_details"

    schemes_detail_id = Column(Integer, primary_key=True, autoincrement=True)
    available_for = Column(String(100), nullable=True)
    name_of_the_scheme = Column(String(255), nullable=True)
    description = Column(Text, nullable=True)
    eligibility_criteria = Column(Text, nullable=True)
    benefits = Column(Text, nullable=True)
    area = Column(String(100), nullable=True)
    documents_required = Column(Text, nullable=True)
    apply_and_link = Column(String(500), nullable=True)
    is_active = Column(Boolean, default=True, nullable=False)

    # Relationship
    multilingual_data = relationship("MultilingualSchemesData", back_populates="scheme")

class MultilingualSchemesData(BaseModel):
    __tablename__ = "multilingual_schemes_data"

    id = Column(Integer, primary_key=True, autoincrement=True)
    scheme_id = Column(Integer, ForeignKey('schemes_details.schemes_detail_id'), nullable=False)
    language_code = Column(String(10), nullable=False)  # 'en', 'hi', 'te', etc.
    name_of_the_scheme = Column(String(255), nullable=True)
    description = Column(Text, nullable=True)
    eligibility_criteria = Column(Text, nullable=True)
    benefits = Column(Text, nullable=True)
    documents_required = Column(Text, nullable=True)

    # Relationship
    scheme = relationship("SchemesDetails", backref="multilingual_data")

class LoanInterestDetails(BaseModel):
    __tablename__ = "loan_interest_details"

    id = Column(Integer, primary_key=True, autoincrement=True)
    scheme_id = Column(Integer, ForeignKey('schemes_details.schemes_detail_id'), nullable=False)
    interest_rate = Column(String(20), nullable=False)
    tenure_years = Column(Integer, nullable=False)
    min_amount = Column(String(50), nullable=True)
    max_amount = Column(String(50), nullable=True)
    processing_fee = Column(String(50), nullable=True)

    # Relationship
    scheme = relationship("SchemesDetails", backref="loan_details")

class OTPDetails(BaseModel):
    __tablename__ = "otp_details"

    id = Column(Integer, primary_key=True, autoincrement=True)
    email = Column(String(255), nullable=False)
    otp_code = Column(String(10), nullable=False)
    otp_type = Column(String(50), nullable=False)  # 'registration', 'password_reset', etc.
    expires_at = Column(DateTime, nullable=False)
    is_used = Column(Boolean, default=False, nullable=False)
