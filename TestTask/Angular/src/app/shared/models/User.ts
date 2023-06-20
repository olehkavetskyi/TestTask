export interface User {
  id: string;
  email: string;
  role: Roles;
}

export enum Roles {
  Regular,
  Admin
}
