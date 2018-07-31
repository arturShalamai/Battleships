export class PlayerregisterModel {
  constructor(
    email: string,
    firstName: string,
    lastName: string,
    nickName: string,
    password: string
  ) {
    this.Email = email;
    this.FirstName = firstName;
    this.LastName = lastName;
    this.NickName = nickName;
    this.Password = password;
  }

  Email: string;
  FirstName: string;
  LastName: string;
  NickName: string;
  Password: string;
}
