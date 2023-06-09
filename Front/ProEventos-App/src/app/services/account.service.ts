import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/models/identity/User';
import { environment } from '@environments/environment';
import { Observable, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  // private currentUserSource = new ReplaySubject<User>(1);
  // public currentUser$ = this.currentUserSource.asObservable();

  baseUrl = environment.apiURL + '/api/account/'
  constructor(private http: HttpClient) { }

  getCurrentUser(): User {
    const userJson = localStorage.getItem('user');
    return userJson !== null ? JSON.parse(userJson) : null;
  }

  public login(model: any): Observable<void>{
    return this.http.post<User>(this.baseUrl +'login', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
        }
      })
    );
  }

  public logout(): void {
    localStorage.removeItem('user');
    // this.currentUserSource.next(undefined);
    // this.currentUserSource.complete();
    //console.log(this.getCurrentUser());
  }

  public setCurrentUser(user: User): void{
    localStorage.setItem('user', JSON.stringify(user));
    //this.currentUserSource.next(user);
    console.log(this.getCurrentUser());
  }

  public register(model: any): Observable<void>{
    return this.http.post<User>(this.baseUrl +'register', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
        }
      })
    );
  }
}
