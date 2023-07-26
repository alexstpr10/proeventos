import { Component, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { environment } from '@environments/environment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent {
  modalRef!: BsModalRef;

  public eventos: Evento[] = [];
  //public eventosFiltrados: Evento[] = [];
  public exibirImg: boolean = true;
  //private _filtroLista: string = "";
  public eventoId =0;
  public pagination = {} as Pagination;

  // public get filtroLista(): string{
  //   return this._filtroLista;
  // }

  // public set filtroLista(value: string)
  // {
  //   this._filtroLista =value;
  //   this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  // }

  public filtrarEventos(evt: any): any {
    this.eventoService.getEventos(this.pagination.currentPage, this.pagination.itemsPerPage, evt.value).subscribe(
      (eventosResp: PaginatedResult<Evento[]>) => {
        this.eventos = eventosResp.result;
        this.pagination = eventosResp.pagination;
      },
      (error: any) => {
        console.log(error);
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos', 'Error!');
      }
    )


    // filtrarPor = filtrarPor.toLocaleLowerCase();
    // return this.eventos.filter(
    //   (evento: { tema: string; local: string; }) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    //   || evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    // )
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router) { }

  public ngOnInit(): void {

    this.pagination = {
      currentPage: 1,
      itemsPerPage: 3,
      totalItems: 1,
    } as Pagination;

    this.getEventos();
  }

  public alterarImagem(): void {
    this.exibirImg = !this.exibirImg;
  }

  public getEventos(): void{

    this.spinner.show();

    this.eventoService.getEventos(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(
      (eventosResp: PaginatedResult<Evento[]>) => {
        this.eventos = eventosResp.result;
        this.pagination = eventosResp.pagination;
      },
      (error: any) => {
        console.log(error);
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos', 'Error!');
      }
    ).add(() => { this.spinner.hide();});
  }

  public openModal(event: any, template: TemplateRef<any>, eventoId: number){
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.getEventos();
  }

  public confirm() : void{
    this.modalRef.hide();
    this.spinner.show();

    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result) => {
        if(result.message == 'Deletado'){
          this.toastr.success(`O evento de código ${this.eventoId} foi excluido com sucesso.`, 'Excluido!');
          this.spinner.hide();
          this.getEventos();
        }
      },
      (error: any) => {
        console.error(error);
        this.toastr.error(`Erro ao tentar deletar o envento ${this.eventoId} erro: ${error} `, 'Erro');
        this.spinner.hide();
      },
      () => this.spinner.hide(),
    );

  }

  public decline() : void{
    this.modalRef.hide();
  }

  public detalheEvento(id: number) : void{
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

  public mostraImagem(imagemURL: string): string{
    return (imagemURL != null && imagemURL != '')
      ? `${environment.apiURL}/resources/images/${imagemURL}`
      : '/assets/no_photo.png';
  }
}
