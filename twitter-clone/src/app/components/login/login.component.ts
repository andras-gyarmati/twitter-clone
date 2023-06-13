import {Component, OnInit} from '@angular/core';
import {UntypedFormBuilder, UntypedFormGroup, Validators} from '@angular/forms';
import {TweetService} from "../../_services/tweet.service";
import {RouterService} from "../../_services/router.service";
import {UserService} from "../../_services/user.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: UntypedFormGroup;
  isError: boolean = false;

  constructor(private fb: UntypedFormBuilder,
              private clientService: TweetService,
              private routerService: RouterService,
              private userService: UserService) {
  }

  async submitForm(): Promise<void> {
    if (this.loginForm.valid) {
      try {
        await this.userService.login(this.loginForm.value);
        this.routerService.routeToTweets();
      } catch (e:any) {
        console.log(e["error"]);
        this.isError = true;
      }
    } else {
      Object.values(this.loginForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({onlySelf: true});
        }
      });
    }
  }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: [null, [Validators.required]],
      password: [null, [Validators.required]],
      remember: [true]
    });
  }

  closeClicked():void {
    this.isError = false;
  }
}
