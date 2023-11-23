import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RedeSocial } from '@app/models/RedeSocial';
import { environment } from '@environments/environment';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RedeSocialService {

  baseURL = environment.apiURL + 'api/redesSociais';

  constructor(private http: HttpClient) { }

  /**
   *
   * @param origem Precisa passar a palavra 'palestrante' ou 'evento' - Escrito em minúsculo
   * @param id
   * @returns
   */
  public getRedesSociais(origem: string, id: number){
    let URL =
      id === 0
        ? `${this.baseURL}/${origem}`
        : `${this.baseURL}/${origem}/${id}`;

    return this.http.get<RedeSocial[]>(URL).pipe(take(1));
  }


}
