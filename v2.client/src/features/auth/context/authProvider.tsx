import React, { createContext, useContext, useEffect, useState, ReactNode } from "react";
import { LoginData, RegisterData, User, UserData, PasswordChangeData, ResetPasswordData } from "@/features/auth";
import {
  changePasswordUser,
  forgotPasswordUser,
  getUserData,
  loadCurrentUser,
  loginUser,
  logoutUser,
  notifyRegisterErrors,
  registerUser,
  resetPasswordUser,
  updateUser,
} from "../services/authService";
import { setAuthToken } from "@/shared";

export interface AuthContextType {
  user: UserData | null;
  loading: boolean;
  login: (data: LoginData) => Promise<boolean>;
  register: (data: RegisterData) => Promise<boolean>;
  logout: () => Promise<void>;
  refreshUser: () => Promise<void>;
  getData: () => Promise<User | null>;
  update: (data: User) => Promise<boolean>;
  changePassword: (data: PasswordChangeData) => Promise<boolean>;
  forgotPassword: (login: string) => Promise<boolean>;
  resetPassword: (data: ResetPasswordData) => Promise<boolean>;
  checkPermission: (permissions: string[]) => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {

  const [user, setUser] = useState<UserData | null>(null);
  const [loading, setLoading] = useState(true);

  const refreshUser = async () => {
    try {
      const data = await loadCurrentUser();
      setUser(data);
    } catch (err: any) {
      if (err.response?.status === 401) {
        setAuthToken(null);
        setUser(null);
      }
    } finally {
      setLoading(false);
    }
  };

  const getData = async (): Promise<User | null> => {
    try {
      return await getUserData();
    } catch (err) {
      console.error("Failed to fetch user data:", err);
      return null;
    }
  };

  const login = async (data: LoginData): Promise<boolean> => {
    try {
      await loginUser(data);
      await refreshUser();
      return true;
    } catch (error) {
      return false;
    }
  };

  const register = async (data: RegisterData): Promise<boolean> => {
    try {
      await registerUser(data);
      await refreshUser();
      return true;
    } catch (error) {
      notifyRegisterErrors(error);
      return false;
    }
  };

  const logout = async () => {
    try {
      await logoutUser();
    } finally {
      setUser(null);
    }
  };

  const update = async (data: User): Promise<boolean> => {
    try {
      await updateUser(data);
      return true;
    } catch (err) {
      console.error("Update failed:", err);
      return false;
    }
  };

  const changePassword = async (data: PasswordChangeData): Promise<boolean> => {
    try {
      await changePasswordUser(data);
      return true;
    } catch (err) {
      console.error("Password change failed:", err);
      return false;
    }
  };

  const forgotPassword = async (login: string): Promise<boolean> => {
    try {
      await forgotPasswordUser(login);
      return true;
    } catch (err) {
      console.error("Forgot password request failed:", err);
      return false;
    }
  };

  const resetPassword = async (data: ResetPasswordData): Promise<boolean> => {
    try {
      await resetPasswordUser(data);
      return true;
    } catch (err) {
      console.error("Password reset failed:", err);
      return false;
    }
  };

  const checkPermission = (permissions: string[]): boolean => {
    if (!user) return false;

    if (user.role === "admin") return true;

    return permissions.some((perm) => user.permissions?.includes(perm));
  };

  useEffect(() => {
    refreshUser();

    const interval = setInterval(refreshUser, 15 * 60 * 1000);

    return () => clearInterval(interval);
  }, []);

  return (
    <AuthContext.Provider
      value={{
        user,
        loading,
        login,
        register,
        logout,
        refreshUser,
        getData,
        update,
        changePassword,
        forgotPassword,
        resetPassword,
        checkPermission,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);

  if (!ctx) {
    throw new Error("useAuth must be used within AuthProvider");
  }

  return ctx;
};
