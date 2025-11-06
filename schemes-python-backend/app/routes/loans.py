from fastapi import APIRouter, Depends, HTTPException, status, Query
from sqlalchemy.orm import Session
from typing import List, Optional
from app.database import get_db
from app.managers.customer_manager import CustomerManager
from app.schemas import (
    LoanDetailsCreate, LoanDetails,
    EMICalculationRequest, EMICalculationResponse,
    AdminCreate, MessageResponse
)
from app.utils.auth import get_current_admin

router = APIRouter(prefix="/loans", tags=["Loans"])

@router.get("/", response_model=List[LoanDetails])
async def get_all_loans(
    current_user = Depends(get_current_admin),
    db: Session = Depends(get_db)
):
    """Get all loan details (Admin only)"""
    try:
        customer_manager = CustomerManager(db)
        loans = customer_manager.get_all_banks_list()
        return loans
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to get loans: {str(e)}"
        )

@router.get("/details", response_model=LoanDetails)
async def get_loan_details(
    bank_name: str = Query(..., description="Bank name"),
    loan_type: str = Query(..., description="Loan type"),
    db: Session = Depends(get_db)
):
    """Get specific loan details"""
    try:
        customer_manager = CustomerManager(db)
        loan = customer_manager.get_loan_details(bank_name, loan_type)
        
        if not loan:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Loan details not found"
            )
        
        return loan
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to get loan details: {str(e)}"
        )

@router.get("/types", response_model=List[str])
async def get_loan_types(
    bank_name: str = Query(..., description="Bank name"),
    db: Session = Depends(get_db)
):
    """Get loan types for a specific bank"""
    try:
        customer_manager = CustomerManager(db)
        loan_types = customer_manager.get_loan_types(bank_name)
        return loan_types
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to get loan types: {str(e)}"
        )

@router.get("/{loan_id}", response_model=LoanDetails)
async def get_loan_by_id(
    loan_id: int,
    current_user = Depends(get_current_admin),
    db: Session = Depends(get_db)
):
    """Get loan details by ID (Admin only)"""
    try:
        customer_manager = CustomerManager(db)
        loan = customer_manager.get_loan_details_by_id(loan_id)
        
        if not loan:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Loan not found"
            )
        
        return loan
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to get loan: {str(e)}"
        )

@router.post("/", response_model=MessageResponse)
async def create_loan(
    loan: LoanDetailsCreate,
    current_user = Depends(get_current_admin),
    db: Session = Depends(get_db)
):
    """Create new loan details (Admin only)"""
    try:
        customer_manager = CustomerManager(db)
        success = customer_manager.add_banks_loan_details(loan)
        
        if success:
            return MessageResponse(
                message="Loan details created successfully",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Failed to create loan details"
            )
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to create loan: {str(e)}"
        )

@router.put("/{loan_id}", response_model=MessageResponse)
async def update_loan(
    loan_id: int,
    loan: LoanDetailsCreate,
    current_user = Depends(get_current_admin),
    db: Session = Depends(get_db)
):
    """Update loan details (Admin only)"""
    try:
        customer_manager = CustomerManager(db)
        success = customer_manager.update_loan_details(loan_id, loan)
        
        if success:
            return MessageResponse(
                message="Loan details updated successfully",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Loan not found or update failed"
            )
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to update loan: {str(e)}"
        )

@router.post("/calculate-emi", response_model=EMICalculationResponse)
async def calculate_emi(
    calculation: EMICalculationRequest,
    db: Session = Depends(get_db)
):
    """Calculate EMI for loan"""
    try:
        customer_manager = CustomerManager(db)
        result = customer_manager.calculate_emi(calculation)
        return result
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to calculate EMI: {str(e)}"
        )
