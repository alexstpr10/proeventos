import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';

@Injectable(
// { providedIn: 'root' }
)
export class EventoService {
  baseURL = 'https://localhost:44304/Eventos';
  //https://localhost:44304/
  //https://localhost:5001

  constructor(private http: HttpClient) { }

  public getEventos() : Observable<Evento[]>
  {
    return this.http.get<Evento[]>(this.baseURL);
  }

  public getEventosByTema(tema: string) : Observable<Evento[]>
  {
    return this.http.get<Evento[]>(`${this.baseURL}/${tema}/tema`);
  }

  public getEventoById(id: number) : Observable<Evento>
  {
    return this.http.get<Evento>(`${this.baseURL}/${id}`);
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

}
