import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { environment } from '@environments/environment';
import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss']
})
export class PalestranteListaComponent implements OnInit {

  public palestrantes: Palestrante[] = [];
  public exibirImg: boolean = true;
  public palestranteId =0;
  public pagination = {} as Pagination;
  termoBuscaChanged: Subject<string> = new Subject<string>();

  constructor(
    private palestranteService: PalestranteService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 3,
      totalItems: 1,
    } as Pagination;

    this.carregarPalestrantes();
  }

  public pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.carregarPalestrantes();
  }

  public carregarPalestrantes(): void{

    this.spinner.show();

    this.palestranteService.getPalestrantes(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(
      (palestrantesResp: PaginatedResult<Palestrante[]>) => {
        this.palestrantes = palestrantesResp.result;
        this.pagination = palestrantesResp.pagination;
      },
      (error: any) => {
        console.log(error);
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os palestrantes', 'Error!');
      }
    ).add(() => { this.spinner.hide();});
  }

  public mostraImagem(imagemURL: string): string{
    return (imagemURL != null && imagemURL != '')
      ? `${environment.apiURL}/resources/perfil/${imagemURL}`
      : '/assets/no_photo.png';
  }

  public filtrarpalestrantes(evt: any): any {
    if (this.termoBuscaChanged.observers.length === 0) {

      this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(
        filtrarPor => {
          this.spinner.show();
          this.palestranteService.getPalestrantes(this.pagination.currentPage, this.pagination.itemsPerPage, filtrarPor).subscribe(
            (palestrantesResp: PaginatedResult<Palestrante[]>) => {
              this.palestrantes = palestrantesResp.result;
              this.pagination = palestrantesResp.pagination;
            },
            (error: any) => {
              console.log(error);
              this.spinner.hide();
              this.toastr.error('Erro ao carregar os palestrantes', 'Error!');
            }
          ).add(() => { this.spinner.hide();});
        }
      )
    }

    this.termoBuscaChanged.next(evt.value);
  }
}
