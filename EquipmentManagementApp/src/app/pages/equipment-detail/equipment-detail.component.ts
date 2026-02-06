import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { ToastrService } from 'ngx-toastr';
import { EquipmentService } from '../../services/equipment.service';
import { CategoryService } from '../../services/category.service';
import { LocationService } from '../../services/location.service';
import { Equipment, UpdateEquipment, EQUIPMENT_STATUSES } from '../../models/equipment.model';
import { Category } from '../../models/category.model';
import { Location } from '../../models/location.model';

@Component({
  selector: 'app-equipment-detail',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    CalendarModule,
    ConfirmDialogModule
  ],
  providers: [ConfirmationService],
  templateUrl: './equipment-detail.component.html',
  styleUrls: ['./equipment-detail.component.scss']
})
export class EquipmentDetailComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private equipmentService = inject(EquipmentService);
  private categoryService = inject(CategoryService);
  private locationService = inject(LocationService);
  private toastr = inject(ToastrService);
  private confirmationService = inject(ConfirmationService);

  equipmentForm!: FormGroup;
  equipment?: Equipment;
  categories: Category[] = [];
  locations: Location[] = [];
  statuses = EQUIPMENT_STATUSES;
  
  loading = false;
  editMode = false;
  equipmentId?: number;
  maxDate = new Date();

  ngOnInit(): void {
    this.initializeForm();
    this.loadCategories();
    this.loadLocations();
    
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.equipmentId = +id;
      this.loadEquipment();
    }
  }

  initializeForm(): void {
    this.equipmentForm = this.fb.group({
      equipmentName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      serialNumber: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
      categoryID: [null, [Validators.required, Validators.min(1)]],
      locationID: [null, [Validators.required, Validators.min(1)]],
      purchaseDate: [null, Validators.required],
      status: ['', Validators.required]
    });

    this.equipmentForm.disable();
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.categories = response.data;
        }
      }
    });
  }

  loadLocations(): void {
    this.locationService.getAll().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.locations = response.data;
        }
      }
    });
  }

  loadEquipment(): void {
    if (!this.equipmentId) return;

    this.loading = true;
    this.equipmentService.getById(this.equipmentId).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success && response.data) {
          this.equipment = response.data;
          this.populateForm(response.data);
        }
      },
      error: () => {
        this.loading = false;
        this.router.navigate(['/equipment']);
      }
    });
  }

  populateForm(equipment: Equipment): void {
    this.equipmentForm.patchValue({
      equipmentName: equipment.equipmentName,
      serialNumber: equipment.serialNumber,
      categoryID: equipment.categoryID,
      locationID: equipment.locationID,
      purchaseDate: new Date(equipment.purchaseDate),
      status: equipment.status
    });
  }

  toggleEditMode(): void {
    this.editMode = !this.editMode;
    if (this.editMode) {
      this.equipmentForm.enable();
    } else {
      this.equipmentForm.disable();
      if (this.equipment) {
        this.populateForm(this.equipment);
      }
    }
  }

  onSubmit(): void {
    if (this.equipmentForm.invalid || !this.equipmentId) {
      this.markFormGroupTouched(this.equipmentForm);
      return;
    }

    const updateData: UpdateEquipment = {
      equipmentID: this.equipmentId,
      ...this.equipmentForm.value
    };

    this.loading = true;
    this.equipmentService.update(updateData).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success) {
          this.toastr.success('Equipment updated successfully', 'Success');
          this.editMode = false;
          this.equipmentForm.disable();
          this.loadEquipment();
        }
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  deleteEquipment(): void {
    if (!this.equipmentId) return;

    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this equipment?',
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.equipmentService.delete(this.equipmentId!).subscribe({
          next: (response) => {
            if (response.success) {
              this.toastr.success('Equipment deleted successfully', 'Success');
              this.router.navigate(['/equipment']);
            }
          }
        });
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/equipment']);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.equipmentForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const control = this.equipmentForm.get(fieldName);
    if (control?.errors) {
      if (control.errors['required']) return `${fieldName} is required`;
      if (control.errors['minlength']) return `Minimum length is ${control.errors['minlength'].requiredLength}`;
      if (control.errors['maxlength']) return `Maximum length is ${control.errors['maxlength'].requiredLength}`;
      if (control.errors['min']) return `Minimum value is ${control.errors['min'].min}`;
    }
    return '';
  }

  getSelectedCategoryName(): string {
    const categoryId = this.equipmentForm.get('categoryID')?.value;
    return this.categories.find(c => c.categoryID === categoryId)?.categoryName || '';
  }

  getSelectedLocationName(): string {
    const locationId = this.equipmentForm.get('locationID')?.value;
    const location = this.locations.find(l => l.locationID === locationId);
    return location ? `${location.locationName} - ${location.building}, Floor ${location.floor}` : '';
  }
}
