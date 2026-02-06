import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { ToastrService } from 'ngx-toastr';
import { EquipmentService } from '../../services/equipment.service';
import { CategoryService } from '../../services/category.service';
import { LocationService } from '../../services/location.service';
import { CreateEquipment, EQUIPMENT_STATUSES } from '../../models/equipment.model';
import { Category } from '../../models/category.model';
import { Location } from '../../models/location.model';

@Component({
  selector: 'app-add-equipment',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    CalendarModule
  ],
  templateUrl: './add-equipment.component.html',
  styleUrls: ['./add-equipment.component.scss']
})
export class AddEquipmentComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private equipmentService = inject(EquipmentService);
  private categoryService = inject(CategoryService);
  private locationService = inject(LocationService);
  private toastr = inject(ToastrService);

  equipmentForm!: FormGroup;
  categories: Category[] = [];
  locations: Location[] = [];
  statuses = EQUIPMENT_STATUSES;
  
  loading = false;
  maxDate = new Date();

  ngOnInit(): void {
    this.initializeForm();
    this.loadCategories();
    this.loadLocations();
  }

  initializeForm(): void {
    this.equipmentForm = this.fb.group({
      equipmentName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      serialNumber: ['', [
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(100),
        Validators.pattern(/^[A-Za-z0-9-]+$/)
      ]],
      categoryID: [null, [Validators.required, Validators.min(1)]],
      locationID: [null, [Validators.required, Validators.min(1)]],
      purchaseDate: [null, [Validators.required, this.noFutureDateValidator]],
      status: ['Active', Validators.required]
    });
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

  onSubmit(): void {
    if (this.equipmentForm.invalid) {
      this.markFormGroupTouched(this.equipmentForm);
      this.toastr.warning('Please fill in all required fields correctly', 'Validation Error');
      return;
    }

    const createData: CreateEquipment = this.equipmentForm.value;

    this.loading = true;
    this.equipmentService.create(createData).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success) {
          this.toastr.success('Equipment added successfully', 'Success');
          this.router.navigate(['/equipment']);
        }
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onReset(): void {
    this.equipmentForm.reset({
      status: 'Active'
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
      if (control.errors['required']) return `${this.formatFieldName(fieldName)} is required`;
      if (control.errors['minlength']) return `Minimum length is ${control.errors['minlength'].requiredLength}`;
      if (control.errors['maxlength']) return `Maximum length is ${control.errors['maxlength'].requiredLength}`;
      if (control.errors['min']) return `Please select a valid ${this.formatFieldName(fieldName)}`;
      if (control.errors['pattern'] && fieldName === 'serialNumber') {
        return 'Serial number can contain letters, numbers, and dashes only';
      }
      if (control.errors['futureDate'] && fieldName === 'purchaseDate') {
        return 'Purchase date cannot be in the future';
      }
    }
    return '';
  }

  private noFutureDateValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;
    const selected = new Date(control.value);
    const today = new Date();
    selected.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);
    return selected > today ? { futureDate: true } : null;
  }

  private formatFieldName(fieldName: string): string {
    return fieldName.replace(/([A-Z])/g, ' $1').trim();
  }
}
