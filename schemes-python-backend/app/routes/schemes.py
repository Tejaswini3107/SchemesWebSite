from fastapi import APIRouter, Depends, HTTPException, status, Query
from sqlalchemy.orm import Session
from typing import List, Optional
from app.database import get_db
from app.managers.customer_manager import CustomerManager
from app.schemas import (
    SchemeDetailsCreate, SchemeDetailsUpdate, SchemeDetails,
    Customer, LoanDetailsCreate, LoanDetails,
    AdminCreate, EMICalculationRequest, EMICalculationResponse,
    MessageResponse
)
from app.utils.auth import get_current_user, get_current_admin

router = APIRouter(prefix="/schemes", tags=["Schemes"])

@router.get("/", response_model=List[dict])
async def get_schemes_list(
    name: str = Query(..., description="Filter schemes by available_for field"),
    lang_code: str = Query("en", description="Language code for multilingual support"),
    db: Session = Depends(get_db)
):
    """Get schemes list with optional language support"""
    try:
        customer_manager = CustomerManager(db)
        schemes = customer_manager.get_schemes_list(name, lang_code)
        return schemes
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to get schemes: {str(e)}"
        )

@router.get("/all", response_model=List[dict])
async def get_all_schemes_list(
    current_user = Depends(get_current_admin),
    db: Session = Depends(get_db)
):
    """Get all schemes (Admin only)"""
    try:
        customer_manager = CustomerManager(db)
        schemes = customer_manager.get_all_schemes_list()
        return schemes
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to get all schemes: {str(e)}"
        )

@router.get("/{scheme_id}", response_model=dict)
async def get_scheme_details(
    scheme_id: int,
    db: Session = Depends(get_db)
):
    """Get specific scheme details"""
    try:
        customer_manager = CustomerManager(db)
        scheme = customer_manager.get_schemes_details(scheme_id)
        
        if not scheme:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Scheme not found"
            )
        
        return scheme
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to get scheme details: {str(e)}"
        )

@router.post("/", response_model=MessageResponse)
async def create_scheme(
    scheme: SchemeDetailsCreate,
    current_user = Depends(get_current_admin),
    db: Session = Depends(get_db)
):
    """Create new scheme (Admin only)"""
    try:
        customer_manager = CustomerManager(db)
        success = customer_manager.add_new_scheme(scheme)
        
        if success:
            return MessageResponse(
                message="Scheme created successfully",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Failed to create scheme"
            )
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to create scheme: {str(e)}"
        )

@router.put("/{scheme_id}", response_model=MessageResponse)
async def update_scheme(
    scheme_id: int,
    scheme_update: SchemeDetailsUpdate,
    current_user = Depends(get_current_admin),
    db: Session = Depends(get_db)
):
    """Update scheme (Admin only)"""
    try:
        customer_manager = CustomerManager(db)
        success = customer_manager.update_scheme(scheme_id, scheme_update)
        
        if success:
            return MessageResponse(
                message="Scheme updated successfully",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Scheme not found or update failed"
            )
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to update scheme: {str(e)}"
        )

@router.delete("/{scheme_id}", response_model=MessageResponse)
async def delete_scheme(
    scheme_id: int,
    current_user = Depends(get_current_admin),
    db: Session = Depends(get_db)
):
    """Delete (deactivate) scheme (Admin only)"""
    try:
        customer_manager = CustomerManager(db)
        success = customer_manager.delete_scheme(scheme_id)
        
        if success:
            return MessageResponse(
                message="Scheme deleted successfully",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Scheme not found or delete failed"
            )
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to delete scheme: {str(e)}"
        )
