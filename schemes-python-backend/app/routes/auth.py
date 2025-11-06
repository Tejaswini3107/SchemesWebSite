from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from typing import List
from app.database import get_db
from app.managers.login_manager import LoginManager
from app.schemas import (
    CustomerLoginRequest, AdminLoginRequest, RegistrationRequest,
    OTPRequest, OTPVerifyRequest, ForgotPasswordRequest, PasswordUpdateRequest,
    LoginResponse, MessageResponse
)
from app.utils.auth import create_access_token, get_current_user

router = APIRouter(prefix="/auth", tags=["Authentication"])

@router.post("/customer/login", response_model=LoginResponse)
async def customer_login(
    login_request: CustomerLoginRequest,
    db: Session = Depends(get_db)
):
    """Customer login endpoint"""
    try:
        login_manager = LoginManager(db)
        customer_id = login_manager.get_customer_login(login_request)
        
        if customer_id:
            access_token = create_access_token(
                data={"sub": str(customer_id), "user_type": "customer"}
            )
            return LoginResponse(
                user_id=customer_id,
                access_token=access_token,
                user_type="customer"
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail="Invalid email or password"
            )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Login failed: {str(e)}"
        )

@router.post("/admin/login", response_model=LoginResponse)
async def admin_login(
    login_request: AdminLoginRequest,
    db: Session = Depends(get_db)
):
    """Admin login endpoint"""
    try:
        login_manager = LoginManager(db)
        admin_id = login_manager.get_admin_login(login_request)
        
        if admin_id:
            access_token = create_access_token(
                data={"sub": str(admin_id), "user_type": "admin"}
            )
            return LoginResponse(
                user_id=admin_id,
                access_token=access_token,
                user_type="admin"
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail="Invalid username or password"
            )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Login failed: {str(e)}"
        )

@router.post("/register", response_model=MessageResponse)
async def register_customer(
    registration: RegistrationRequest,
    db: Session = Depends(get_db)
):
    """Customer registration endpoint"""
    try:
        login_manager = LoginManager(db)
        success = login_manager.customer_registration(registration)
        
        if success:
            return MessageResponse(
                message="Registration successful. Please verify your email to activate your account.",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Registration failed"
            )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Registration failed: {str(e)}"
        )

@router.post("/send-otp", response_model=MessageResponse)
async def send_otp(
    otp_request: OTPRequest,
    db: Session = Depends(get_db)
):
    """Send OTP for verification"""
    try:
        login_manager = LoginManager(db)
        success = login_manager.send_otp(otp_request)
        
        if success:
            return MessageResponse(
                message="OTP sent successfully",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Failed to send OTP"
            )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Send OTP failed: {str(e)}"
        )

@router.post("/verify-otp", response_model=MessageResponse)
async def verify_otp(
    verify_request: OTPVerifyRequest,
    db: Session = Depends(get_db)
):
    """Verify OTP"""
    try:
        login_manager = LoginManager(db)
        is_verified = login_manager.verify_otp(verify_request)
        
        if is_verified:
            return MessageResponse(
                message="OTP verified successfully",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Invalid or expired OTP"
            )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"OTP verification failed: {str(e)}"
        )

@router.post("/forgot-password", response_model=MessageResponse)
async def forgot_password(
    forgot_request: ForgotPasswordRequest,
    db: Session = Depends(get_db)
):
    """Initiate forgot password process"""
    try:
        login_manager = LoginManager(db)
        success = login_manager.forgot_password(forgot_request)
        
        if success:
            return MessageResponse(
                message="Password reset OTP sent to your email",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Email address not found"
            )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Forgot password failed: {str(e)}"
        )

@router.post("/update-password", response_model=MessageResponse)
async def update_password(
    password_request: PasswordUpdateRequest,
    db: Session = Depends(get_db)
):
    """Update password after OTP verification"""
    try:
        login_manager = LoginManager(db)
        success = login_manager.update_password(password_request)
        
        if success:
            return MessageResponse(
                message="Password updated successfully",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Invalid OTP or password update failed"
            )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Password update failed: {str(e)}"
        )
