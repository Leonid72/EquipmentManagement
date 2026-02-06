import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastr = inject(ToastrService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unexpected error occurred';

      if (error.error instanceof ErrorEvent) {
        // Client-side error
        errorMessage = `Error: ${error.error.message}`;
      } else {
        // Server-side error
        if (error.status === 0) {
          errorMessage = 'Unable to connect to the server. Please check your internet connection.';
        } else if (error.status === 400) {
          errorMessage = error.error?.message || 'Bad request';
          if (error.error?.errors && error.error.errors.length > 0) {
            error.error.errors.forEach((err: string) => {
              toastr.error(err, 'Validation Error');
            });
            return throwError(() => error);
          }
        } else if (error.status === 404) {
          errorMessage = 'Resource not found';
        } else if (error.status === 500) {
          errorMessage = 'Internal server error. Please try again later.';
        } else {
          errorMessage = error.error?.message || `Error: ${error.status}`;
        }
      }

      toastr.error(errorMessage, 'Error');
      return throwError(() => error);
    })
  );
};
