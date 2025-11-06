from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from typing import List
from app.database import get_db
from app.managers.customer_manager import CustomerManager
from app.schemas import (
    Customer, LoanDetailsCreate, LoanDetails,
    EMICalculationRequest, EMICalculationResponse,
    MessageResponse
)
from app.utils.auth import get_current_user, get_current_admin

router = APIRouter(prefix="/customers", tags=["Customers"])

@router.get("/", response_model=List[Customer])
async def get_customers_list(
    current_user = Depends(get_current_admin),
    db: Session = Depends(get_db)
):
    """Get all customers (Admin only)"""
    try:
        customer_manager = CustomerManager(db)
        customers = customer_manager.get_customers_list()
        return customers
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to get customers: {str(e)}"
        )

@router.put("/{customer_id}/status", response_model=MessageResponse)
async def update_customer_status(
    customer_id: int,
    current_user = Depends(get_current_admin),
    db: Session = Depends(get_db)
):
    """Update customer status (Admin only)"""
    try:
        customer_manager = CustomerManager(db)
        success = customer_manager.update_customer_status(customer_id)
        
        if success:
            return MessageResponse(
                message="Customer status updated successfully",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Customer not found"
            )
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to update customer status: {str(e)}"
        )

@router.get("/{customer_id}/profile")
async def get_customer_profile(
    customer_id: int,
    current_user = Depends(get_current_user),
    db: Session = Depends(get_db)
):
    """Get customer profile details"""
    try:
        customer_manager = CustomerManager(db)
        customers = customer_manager.get_customers_list()
        
        customer = next((c for c in customers if c.customer_id == customer_id), None)
        
        if not customer:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Customer not found"
            )
        
        return customer
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to get customer profile: {str(e)}"
        )
