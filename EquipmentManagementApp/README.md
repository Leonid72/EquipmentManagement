# Equipment Management System - Angular 19

A modern, responsive equipment management application built with Angular 19, PrimeNG, and RxJS.

## Features

### Equipment List View

- Display all equipment in a sortable, paginated table
- Show: Name, Serial Number, Category, Location, Status
- Filter by category and status
- Search by equipment name
- Filter by purchase date range
- Pagination (10, 25, or 50 items per page)
- Delete equipment with confirmation dialog

### Equipment Detail View

- View complete equipment information
- Edit equipment details inline
- Delete equipment with confirmation
- Responsive card-based layout

### Add New Equipment Form

- Comprehensive form with all required fields
- Dropdown selectors for Category and Location
- Date picker with validation (purchase date ≤ today)
- Form validation with error messages
- Success/error notifications via Toastr

## Technical Stack

- **Angular 19** - Standalone components (no NgModules)
- **PrimeNG 19** - UI component library
- **RxJS** - Reactive programming with Observables
- **TypeScript** - Strict typing throughout
- **ngx-toastr** - Toast notifications
- **SCSS** - Component styling

## Project Structure

```
src/
├── app/
│   ├── models/                    # TypeScript interfaces
│   │   ├── equipment.model.ts
│   │   ├── category.model.ts
│   │   ├── location.model.ts
│   │   └── api-response.model.ts
│   ├── services/                  # API services
│   │   ├── equipment.service.ts
│   │   ├── category.service.ts
│   │   └── location.service.ts
│   ├── pages/                     # Page components
│   │   ├── equipment-list/
│   │   │   ├── equipment-list.component.ts
│   │   │   ├── equipment-list.component.html
│   │   │   └── equipment-list.component.scss
│   │   ├── equipment-detail/
│   │   │   ├── equipment-detail.component.ts
│   │   │   ├── equipment-detail.component.html
│   │   │   └── equipment-detail.component.scss
│   │   └── add-equipment/
│   │       ├── add-equipment.component.ts
│   │       ├── add-equipment.component.html
│   │       └── add-equipment.component.scss
│   ├── shared/
│   │   └── interceptors/
│   │       └── error.interceptor.ts  # Global error handler
│   ├── app.component.ts
│   ├── app.component.html
│   ├── app.component.scss
│   ├── app.routes.ts
│   └── app.config.ts
├── environments/
│   ├── environment.ts
│   └── environment.prod.ts
├── styles.scss
└── main.ts
```

## Setup Instructions

### Prerequisites

- Node.js 18.x or higher
- npm 9.x or higher

### Installation

1. Install dependencies:

```bash
npm install
```

2. Configure API URL:
   - Open `src/environments/environment.ts`
   - Update the `apiUrl` with your backend API URL:

   ```typescript
   export const environment = {
     production: false,
     apiUrl: "https://your-api-url.com/api",
   };
   ```

3. Start the development server:

```bash
npm start
```

4. Open your browser and navigate to `http://localhost:4200`

## API Integration

The application expects the following API endpoints:

### Equipment Endpoints

- `GET /api/Equipments` - Get paginated equipment list with filters
- `GET /api/Equipments/{id}` - Get equipment by ID
- `POST /api/Equipments` - Create new equipment
- `PUT /api/Equipments/{id}` - Update equipment
- `DELETE /api/Equipments/{id}` - Delete equipment

### Category Endpoints

- `GET /api/Categories` - Get all categories

### Location Endpoints

- `GET /api/Locations` - Get all locations

### Expected Response Format

```typescript
{
  success: boolean,
  message: string,
  data?: T,
  errors: string[]
}
```

## Features Implemented

### ✅ Component Architecture

- Standalone components (Angular 19 best practice)
- Separate HTML and SCSS files for each component
- Service layer for API communication
- Reactive forms with validation

### ✅ State Management

- Observable-based reactive state
- RxJS operators for data transformation
- Proper subscription management

### ✅ Error Handling

- Global HTTP interceptor for error handling
- Toastr notifications for user feedback
- Form validation with error messages
- Loading states throughout the application

### ✅ Responsive Design

- Mobile-friendly layouts
- Responsive tables and forms
- Adaptive navigation
- Touch-friendly UI elements

### ✅ User Experience

- Loading indicators
- Confirmation dialogs for destructive actions
- Toast notifications for feedback
- Clear form validation messages
- Helpful placeholder text

## Development

### Build for Production

```bash
npm run build
```

### Run Tests

```bash
npm test
```

### Code Formatting

The project follows Angular style guide and TypeScript best practices.

## Key Dependencies

```json
{
  "@angular/core": "^19.0.0",
  "primeng": "^19.0.0",
  "primeicons": "^7.0.0",
  "ngx-toastr": "^19.0.0",
  "rxjs": "~7.8.0"
}
```

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## Additional Notes

### Status Values

The application supports four equipment statuses:

- **Active** - Equipment is in use
- **InMaintenance** - Equipment is being serviced
- **OutOfService** - Equipment is not operational
- **Retired** - Equipment is no longer in service

### Form Validations

- Equipment Name: 3-200 characters
- Serial Number: 5-100 characters
- Category: Required selection
- Location: Required selection
- Purchase Date: Required, cannot be in future
- Status: Required selection

### PrimeNG Components Used

- Table with pagination
- Dropdown
- Calendar
- Button
- Card
- InputText
- ConfirmDialog

## License

MIT License - feel free to use this project for your own purposes.

## Support

For issues or questions, please refer to the official documentation:

- [Angular Documentation](https://angular.dev)
- [PrimeNG Documentation](https://primeng.org)
- [RxJS Documentation](https://rxjs.dev)
