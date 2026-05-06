import React, { createContext, useContext, useEffect, useState, ReactNode } from "react";
import { useApiConnect } from "@/shared/context/apiConnect";
import { LoginData, RegisterData, User, UserData, PasswordChangeData } from "@/features/auth";
import { notify } from "@/shared/components/NotificationListener";
import { getNotify } from "@/lib/notifyProvider";
import {call} from '@/shared';

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
  checkPermission: (permissions: string[]) => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {

  const [user, setUser] = useState<UserData | null>(null);
  const [loading, setLoading] = useState(true);

  const refreshUser = async () => {
    try {
      const data = await call<UserData>(api => api.authApi.getAuthMe,{});
      setUser(data?.userId ? data : null);
    } catch (err: any) {
      if (err.response?.status === 401) {
        setUser(null); // only logout on unauthorized
      }
    } finally {
      setLoading(false);
    }
  };

  const getData = async (): Promise<User | null> => {
    try {
      const data  = await call<User>(api => api.authApi.getAuthData,{}); 
      return data;
    } catch (err) {
      console.error("Failed to fetch user data:", err);
      return null;
    }
  };

  const login = async (data: LoginData): Promise<boolean> => {
    try {
      await call(api => api.authApi.createAuthLogin,data);
      await refreshUser();
      return true;
    } catch (error) {
      return false;
    }
  };

  const register = async (data: RegisterData): Promise<boolean> => {
    try {
      await call(api => api.authApi.createAuthRegister,data);
      await refreshUser();
      return true;
    } catch (error) {
      error.response.data.map((err: string) => notify('error', getNotify(err)));
      return false;
    }
  };

  const logout = async () => {
    try {
      await call(api =>api.authApi.createAuthLogout,{});
    } finally {
      setUser(null);
    }
  };

  const update = async (data: User): Promise<boolean> => {
    try {
      await call(api => api.authApi.updateAuth,{id:user.id, body:data});
      return true;
    } catch (err) {
      console.error("Update failed:", err);
      return false;
    }
  };

  const changePassword = async (data: PasswordChangeData): Promise<boolean> => {
    try {
      await call(api => api.authApi.updateAuth,{id:user.id, body:data});
      return true;
    } catch (err) {
      console.error("Password change failed:", err);
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