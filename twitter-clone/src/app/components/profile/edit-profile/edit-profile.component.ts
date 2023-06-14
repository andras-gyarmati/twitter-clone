import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import {RouterService} from "../../../_services/router.service";
import { NzFormTooltipIcon } from 'ng-zorro-antd/form';
import {NzUploadChangeParam} from "ng-zorro-antd/upload";
import {User} from "../../../models/user";
import {UserService} from "../../../_services/user.service";
import {NzMessageService} from "ng-zorro-antd/message";

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  validateForm!: UntypedFormGroup;
  captchaTooltipIcon: NzFormTooltipIcon = {
    type: 'info-circle',
    theme: 'twotone'
  };

  constructor(private routerService: RouterService,
              private fb: UntypedFormBuilder,
              public userService: UserService,
              private msg: NzMessageService) {}

  public myDatas: User | undefined = this.userService.getLoggedInUser();

  async submitForm(): Promise<void> {
    if (this.validateForm.valid) {
      console.log('submit', this.validateForm.value);
      await this.userService.updateUser(this.validateForm.value);
      this.routerService.routeToProfile(this.userService.getLoggedInUser()?.username?? "");
    } else {
      Object.values(this.validateForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  inputValue: string = "";

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      birthDate: [this.myDatas?.birthDate],
      bio: [this.myDatas?.bio],
      picture: [null]
    });
  }

  handleChange(info: NzUploadChangeParam): void {
    if (info.file.status !== 'uploading') {
      console.log(info.file, info.fileList);
    }
    if (info.file.status === 'done') {
      this.msg.success(`${info.file.name} file uploaded successfully`);
    } else if (info.file.status === 'error') {
      this.msg.error(`${info.file.name} file upload failed.`);
    }
  }
}
