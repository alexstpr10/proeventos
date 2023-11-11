import { Component, OnInit } from '@angular/core';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  userUpdate = {} as UserUpdate;
  public imagemURL = '';
  public file: File;

  constructor(private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private accountService: AccountService) { }

  ngOnInit() {
  }

  public setFormValue(usuario: UserUpdate): void {
    this.userUpdate = usuario;
  }

  public onFileChange(ev: any): void{
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files[0];
    reader.readAsDataURL(this.file);

    this.uploadImage();
  }

  public uploadImage(): void {
    this.spinner.show();
    this.accountService.postUpload(this.file).subscribe(
      () => {
        //this.carregarEvento();
        this.toastr.success('Imagem atualizada com sucesso', 'Sucesso!');
      },
      (error: any) => {
        this.toastr.error('Erro ao fazer upload de imagem', 'Erro!');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

}
