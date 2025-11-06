from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.database import get_db
from app.managers.customer_manager import CustomerManager
from app.schemas import AdminCreate, MessageResponse
from app.utils.auth import get_current_admin

router = APIRouter(prefix="/admin", tags=["Admin"])

@router.post("/create", response_model=MessageResponse)
async def create_admin(
    admin_data: AdminCreate,
    current_user = Depends(get_current_admin),
    db: Session = Depends(get_db)
):
    """Create new admin (Admin only)"""
    try:
        customer_manager = CustomerManager(db)
        success = customer_manager.create_new_admin(admin_data)
        
        if success:
            return MessageResponse(
                message="Admin created successfully",
                success=True
            )
        else:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Failed to create admin"
            )
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to create admin: {str(e)}"
        )
