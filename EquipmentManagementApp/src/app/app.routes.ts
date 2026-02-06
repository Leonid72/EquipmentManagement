import { Routes } from '@angular/router';
import { EquipmentListComponent } from './pages/equipment-list/equipment-list.component';
import { EquipmentDetailComponent } from './pages/equipment-detail/equipment-detail.component';
import { AddEquipmentComponent } from './pages/add-equipment/add-equipment.component';

export const routes: Routes = [
  { path: '', redirectTo: '/equipment', pathMatch: 'full' },
  { path: 'equipment', component: EquipmentListComponent },
  { path: 'equipment/new', component: AddEquipmentComponent },
  { path: 'equipment/:id', component: EquipmentDetailComponent },
  { path: '**', redirectTo: '/equipment' }
];
