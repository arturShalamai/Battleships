export class PlayerRegisterModel {

  constructor(
    email: string,
    firstName: string,
    lastName: string,
    nickName: string,
    password: string,
    confirmPassword: string
  ) {
    this.Email = email;
    this.FirstName = firstName;
    this.LastName = lastName;
    this.NickName = nickName;
    this.Password = password;
    this.ConfirmPassword = confirmPassword;
  }

  Email: string;
  FirstName: string;
  LastName: string;
  NickName: string;
  Password: string;
  ConfirmPassword: string;
}
