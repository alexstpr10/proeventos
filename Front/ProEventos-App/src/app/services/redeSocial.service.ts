import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RedeSocial } from '@app/models/RedeSocial';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RedeSocialService {

  baseURL = environment.apiURL + '/api/RedesSociais';

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

    console.log(URL);

    return this.http.get<RedeSocial[]>(URL).pipe(take(1));
  }

  /**
   *
   * @param origem Precisa passar a palavra 'palestrante' ou 'evento' - Escrito em minúsculo
   * @param id Precisa passar o PalestranteId ou o EnventoId dependendo da sua Origem
   * @param redesSociais Precisa adicionar redes sociais organizadas por em RedeSocial[];
   * @returns
   */
    public saveRedesSociais(origem: string, id: number, redesSociais: RedeSocial[]) : Observable<RedeSocial[]>
    {
      let URL =
        id === 0
          ? `${this.baseURL}/${origem}`
          : `${this.baseURL}/${origem}/${id}`;

      return this.http.put<RedeSocial[]>(URL, redesSociais).pipe(take(1));
    }

    /**
   *
   * @param origem Precisa passar a palavra 'palestrante' ou 'evento' - Escrito em minúsculo
   * @param id Precisa passar o PalestranteId ou o EnventoId dependendo da sua Origem
   * @param redeSocialId Precisa usar o id da Rede Social;
   * @returns
   */
    public deleteRedeSocial(origem: string, id: number, redeSocialId: number) : Observable<any>
    {
      let URL =
        id === 0
          ? `${this.baseURL}/${origem}/${redeSocialId}`
          : `${this.baseURL}/${origem}/${id}/${redeSocialId}`;

      return this.http.delete<any>(URL).pipe(take(1));
    }

}
