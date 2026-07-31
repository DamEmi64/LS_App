import { call, setAuthToken, setAuthTokens, type AuthToken } from "@/shared";
import { notify } from "@/shared/components/NotificationListener";
import { getNotify } from "@/lib/notifyProvider";

import { LoginData, PasswordChangeData, RegisterData, ResetPasswordData, User, UserData } from "../index";

export async function loadCurrentUser() {
  const data = await call<UserData>(api => api.authApi.getMe, {});
  return data?.userId ? data : null;
}

export async function getUserData() {
  return call<User>(api => api.authApi.getUser, {});
}

export async function loginUser(data: LoginData) {
  const token = await call<AuthToken>(api => api.authApi.createLogin, { loginModel: data });
  setAuthTokens(token);
  return token;
}

export async function registerUser(data: RegisterData) {
  const token = await call<AuthToken>(api => api.authApi.createRegister, { registerModel: data });
  setAuthTokens(token);
  return token;
}

export async function logoutUser() {
  try {
    await call(api => api.authApi.createLogout, {});
  } finally {
    setAuthToken(null);
  }
}

export async function updateUser(data: User) {
  return call(api => api.authApi.update, { user: data });
}

export async function changePasswordUser(data: PasswordChangeData) {
  return call(api => api.authApi.update, { user: data });
}

export async function forgotPasswordUser(login: string) {
  return call(api => api.authApi.getForgotPassword, { username: login });
}

export async function resetPasswordUser(data: ResetPasswordData) {
  return call(api => api.authApi.createResetPassword, { resetPasswordModel: data });
}

export function notifyRegisterErrors(error: any) {
  if (error?.response?.data) {
    error.response.data.forEach((err: string) => notify("error", getNotify(err)));
  }
}
