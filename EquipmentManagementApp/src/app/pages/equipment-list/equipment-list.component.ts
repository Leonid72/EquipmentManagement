import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { ToastrService } from 'ngx-toastr';
import { EquipmentService } from '../../services/equipment.service';
import { CategoryService } from '../../services/category.service';
import { Equipment, EquipmentSearch, EQUIPMENT_STATUSES } from '../../models/equipment.model';
import { Category } from '../../models/category.model';
import { PagedResult } from '../../models/api-response.model';

@Component({
  selector: 'app-equipment-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    CalendarModule,
    ConfirmDialogModule
  ],
  providers: [ConfirmationService],
  templateUrl: './equipment-list.component.html',
  styleUrls: ['./equipment-list.component.scss']
})
export class EquipmentListComponent implements OnInit {
  private equipmentService = inject(EquipmentService);
  private categoryService = inject(CategoryService);
  private toastr = inject(ToastrService);
  private confirmationService = inject(ConfirmationService);
  private router = inject(Router);

  equipment: Equipment[] = [];
  categories: Category[] = [];
  statuses = EQUIPMENT_STATUSES;
  
  loading = false;
  totalRecords = 0;
  
  searchParams: EquipmentSearch = {
    pageNumber: 1,
    pageSize: 10,
    sortBy: 'equipmentName',
    sortDirection: 'ASC'
  };

  filters = {
    searchTerm: '',
    categoryID: null as number | null,
    status: null as string | null,
    purchaseDateFrom: null as Date | null,
    purchaseDateTo: null as Date | null
  };

  ngOnInit(): void {
    this.loadCategories();
    this.loadEquipment();
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: (response) => {
        this.categories = response.success && response.data ? response.data : [];
      },
      error: () => {
        // Error handling is done by interceptor
      }
    });
  }

  loadEquipment(): void {
    this.loading = true;
    
    const params: EquipmentSearch = {
      ...this.searchParams,
      searchTerm: this.filters.searchTerm || undefined,
      categoryID: this.filters.categoryID || undefined,
      status: this.filters.status || undefined,
      purchaseDateFrom: this.filters.purchaseDateFrom || undefined,
      purchaseDateTo: this.filters.purchaseDateTo || undefined
    };

    this.equipmentService.getAll(params).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success && response.data) {
          this.equipment = response.data.items;
          this.totalRecords = response.data.totalCount;
        }
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onPageChange(event: any): void {
    this.searchParams.pageNumber = event.first / event.rows + 1;
    this.searchParams.pageSize = event.rows;
    this.loadEquipment();
  }

  onSearch(): void {
    this.searchParams.pageNumber = 1;
    this.loadEquipment();
  }

  onClearFilters(): void {
    this.filters = {
      searchTerm: '',
      categoryID: null,
      status: null,
      purchaseDateFrom: null,
      purchaseDateTo: null
    };
    this.searchParams.pageNumber = 1;
    this.loadEquipment();
  }

  viewDetails(equipment: Equipment): void {
    this.router.navigate(['/equipment', equipment.equipmentID]);
  }

  addNew(): void {
    this.router.navigate(['/equipment/new']);
  }

  deleteEquipment(equipment: Equipment): void {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete equipment "${equipment.equipmentName}"?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.equipmentService.delete(equipment.equipmentID).subscribe({
          next: (response) => {
            if (response.success) {
              this.toastr.success('Equipment deleted successfully', 'Success');
              this.loadEquipment();
            }
          },
          error: () => {
            // Error handling is done by interceptor
          }
        });
      }
    });
  }

  getStatusClass(status: string): string {
    const statusClasses: { [key: string]: string } = {
      'Active': 'status-active',
      'InMaintenance': 'status-maintenance',
      'OutOfService': 'status-out-of-service',
      'Retired': 'status-retired'
    };
    return statusClasses[status] || '';
  }

  formatStatus(status: string): string {
    return status.replace(/([A-Z])/g, ' $1').trim();
  }
}
