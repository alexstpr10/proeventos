import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginatedResult } from '@app/models/Pagination';
import { Palestrante } from '@app/models/Palestrante';
import { Constants } from '@app/util/constants';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PalestranteService {

  baseURL = `${Constants.BASE_URL}/Palestrantes`;
  //https://localhost:44304/
  //https://localhost:5001

  constructor(private http: HttpClient) { }

  public getPalestrantes(page?: number, itensPerPage?: number, term?: string) : Observable<PaginatedResult<Palestrante[]>>
  {
    const paginatedResult: PaginatedResult<Palestrante[]> = new PaginatedResult<Palestrante[]>();

    let params = new HttpParams;

    if( page !== undefined && itensPerPage !== undefined){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itensPerPage.toString());
    }

    if( term !== undefined && term !== '')
      params = params.append('term', term);

    return this.http.get<Palestrante[]>(this.baseURL + '/all', {observe: 'response', params})
      .pipe(
        take(1),
        map((response) => {
        paginatedResult.result = response.body;
        if(response.headers.has('Pagination')){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination')??'');
        }

        return paginatedResult;

      }));
  }

  public getPalestrante() : Observable<Palestrante>
  {
    return this.http.get<Palestrante>(`${this.baseURL}`)
      .pipe(take(1));
  }

  public post() : Observable<Palestrante>
  {
    return this.http.post<Palestrante>(this.baseURL, {} as Palestrante).pipe(take(1));
  }

  public put(palestrante: Palestrante) : Observable<Palestrante>
  {
    return this.http.put<Palestrante>(`${this.baseURL}`, palestrante);
  }
}
