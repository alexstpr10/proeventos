import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';
import { map, take } from 'rxjs/operators';
import { Constants } from '@app/util/constants';
import { PaginatedResult } from '@app/models/Pagination';
import { JsonPipe } from '@angular/common';
@Injectable(
// { providedIn: 'root' }
)
export class EventoService {
  baseURL = `${Constants.BASE_URL}/Eventos`;
  //https://localhost:44304/
  //https://localhost:5001

  constructor(private http: HttpClient) { }

  public getEventos(page?: number, itensPerPage?: number, term?: string) : Observable<PaginatedResult<Evento[]>>
  {
    const paginatedResult: PaginatedResult<Evento[]> = new PaginatedResult<Evento[]>();

    let params = new HttpParams;

    if( page !== undefined && itensPerPage !== undefined){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itensPerPage.toString());
    }

    if( term !== undefined && term !== '')
      params = params.append('term', term);

    return this.http.get<Evento[]>(this.baseURL, {observe: 'response', params})
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

  public getEventosByTema(tema: string) : Observable<Evento[]>
  {
    return this.http.get<Evento[]>(`${this.baseURL}/${tema}/tema`);
  }

  public getEventoById(id: number) : Observable<Evento>
  {
    return this.http.get<Evento>(`${this.baseURL}/${id}`)
      .pipe(take(1));
  }

  public postEvento(evento: Evento) : Observable<Evento>
  {
    return this.http.post<Evento>(this.baseURL, evento);
  }

  public putEvento(id: number, evento: Evento) : Observable<Evento>
  {
    return this.http.put<Evento>(`${this.baseURL}/${id}`, evento);
  }

  public deleteEvento(id: number) : Observable<any>
  {
    return this.http.delete(`${this.baseURL}/${id}`);
  }

  public postUpload(eventoId: number, file: File): Observable<Evento> {
    const fileToUpload = file;
    const formData = new FormData();
    formData.append('file', fileToUpload);

    return this.http
      .post<Evento>(`${this.baseURL}/upload-image/${eventoId}`, formData)
      .pipe(take(1));
  }

}
