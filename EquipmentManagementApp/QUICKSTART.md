# Quick Start Guide

This guide will help you get the Equipment Management System up and running quickly.

## Prerequisites

Make sure you have installed:
- **Node.js** (v18 or higher) - [Download here](https://nodejs.org/)
- **npm** (comes with Node.js)
- A code editor like **VS Code** (recommended)

## Step-by-Step Setup

### 1. Install Dependencies

Open a terminal in the project directory and run:

```bash
npm install
```

This will install all required packages including Angular, PrimeNG, and other dependencies.

### 2. Configure API URL

Edit the file `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'  // Replace with your API URL
};
```

**Important:** Make sure your backend API is running and accessible at this URL.

### 3. Start the Development Server

```bash
npm start
```

or

```bash
ng serve
```

The application will start on `http://localhost:4200`

### 4. Open in Browser

Navigate to: **http://localhost:4200**

You should see the Equipment Management System home page.

## Available Scripts

| Command | Description |
|---------|-------------|
| `npm start` | Start development server |
| `npm run build` | Build for production |
| `npm test` | Run unit tests |
| `ng serve` | Start development server (alternative) |
| `ng build` | Build for production (alternative) |

## Application Features

### Equipment List Page (`/equipment`)
- View all equipment in a paginated table
- Search by name or serial number
- Filter by category, status, and purchase date
- Click "View" icon to see details
- Click "Delete" icon to remove equipment
- Click "Add New Equipment" to create new entries

### Add Equipment Page (`/equipment/new`)
- Fill in all required fields
- Select category from dropdown
- Select location from dropdown
- Choose purchase date (cannot be in future)
- Select equipment status
- Click "Add Equipment" to save

### Equipment Detail Page (`/equipment/:id`)
- View all equipment information
- Click "Edit" to modify details
- Make changes and click "Save Changes"
- Click "Delete" to remove equipment
- Click "Cancel" to discard changes

## Folder Structure Overview

```
src/
├── app/
│   ├── models/           # Data models/interfaces
│   ├── services/         # API communication services
│   ├── pages/           # Main page components
│   ├── shared/          # Shared utilities (interceptors)
│   └── app.component.*  # Root component
├── environments/        # Environment configurations
└── styles.scss         # Global styles
```

## API Requirements

Your backend API should provide these endpoints:

- `GET /api/equipment` - List equipment (with pagination)
- `GET /api/equipment/{id}` - Get single equipment
- `POST /api/equipment` - Create equipment
- `PUT /api/equipment/{id}` - Update equipment
- `DELETE /api/equipment/{id}` - Delete equipment
- `GET /api/categories` - List categories
- `GET /api/locations` - List locations

See `API_INTEGRATION.md` for detailed API specifications.

## Common Issues & Solutions

### Issue: "Cannot connect to API"
**Solution:** 
- Verify your backend is running
- Check the API URL in `environment.ts`
- Ensure CORS is enabled on your backend

### Issue: "npm install fails"
**Solution:**
- Delete `node_modules` folder
- Delete `package-lock.json`
- Run `npm install` again

### Issue: "Port 4200 is already in use"
**Solution:**
```bash
ng serve --port 4201
```

### Issue: "PrimeNG styles not loading"
**Solution:**
- Make sure `angular.json` includes PrimeNG styles in the styles array
- Clear browser cache and restart dev server

## Build for Production

To create a production build:

```bash
npm run build
```

The build artifacts will be stored in the `dist/` directory.

To serve the production build locally:

```bash
npx http-server dist/equipment-management-app/browser -p 8080
```

## Next Steps

1. **Customize Styles**: Edit `src/styles.scss` for global styles
2. **Add Authentication**: Implement login/logout functionality
3. **Add More Features**: Extend with maintenance tracking, reports, etc.
4. **Configure Production**: Update `environment.prod.ts` with production API URL
5. **Deploy**: Deploy to hosting service (Netlify, Vercel, Firebase, etc.)

## Getting Help

- **Angular Documentation**: https://angular.dev
- **PrimeNG Documentation**: https://primeng.org
- **TypeScript Documentation**: https://www.typescriptlang.org/docs/

## Development Tips

### Hot Reload
The application supports hot reload - changes to code will automatically refresh the browser.

### Browser DevTools
- Open Chrome DevTools (F12)
- Use Network tab to debug API calls
- Use Console tab to view error messages

### VS Code Extensions (Recommended)
- Angular Language Service
- Angular Snippets
- ESLint
- Prettier
- Angular Console

## Testing the Application

### Manual Testing Checklist

✅ **Equipment List:**
- [ ] Table displays equipment
- [ ] Pagination works
- [ ] Search filters results
- [ ] Category filter works
- [ ] Status filter works
- [ ] Date range filter works
- [ ] Delete shows confirmation
- [ ] Delete removes equipment

✅ **Add Equipment:**
- [ ] All fields are present
- [ ] Validation messages appear
- [ ] Cannot submit with invalid data
- [ ] Success message on save
- [ ] Redirects to list after save

✅ **Equipment Detail:**
- [ ] All information displays
- [ ] Edit mode enables fields
- [ ] Changes can be saved
- [ ] Cancel discards changes
- [ ] Delete shows confirmation

## Support

For issues or questions:
1. Check the `API_INTEGRATION.md` file
2. Review the `README.md` file
3. Check browser console for errors
4. Verify backend API is working

Happy coding! 🚀
