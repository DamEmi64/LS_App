import React, { createContext, useContext, useEffect, useState, ReactNode } from "react";
import { useApiConnect } from "@/shared/context/apiConnect";
import { LoginData, RegisterData, User, UserData, PasswordChangeData } from "@/features/auth";
import { notify } from "@/shared/components/NotificationListener";
import { getNotify } from "@/lib/notifyProvider";

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
  const api = useApiConnect();

  const [user, setUser] = useState<UserData | null>(null);
  const [loading, setLoading] = useState(true);

  const refreshUser = async () => {
    try {
      const { data } = await api.get<UserData | null>("auth_me");
      setUser(data?.userId ? data : null);
    } catch {
      setUser(null);
    } finally {
      setLoading(false);
    }
  };

  const getData = async (): Promise<User | null> => {
    try {
      const { data } = await api.get<User>("auth_full");
      return data;
    } catch (err) {
      console.error("Failed to fetch user data:", err);
      return null;
    }
  };

  const login = async (data: LoginData): Promise<boolean> => {
    try {
      await api.post("auth_login", data);
      await refreshUser();
      return true;
    } catch (error) {
      error.response.data.map((err: string) => notify('error', getNotify(err))) ?? notify('error', 'Login failed');
      return false;
    }
  };

  const register = async (data: RegisterData): Promise<boolean> => {
    try {
      await api.post("auth_register", data);
      await refreshUser();
      return true;
    } catch (error) {
      error.response.data.map((err: string) => notify('error', getNotify(err)));
      return false;
    }
  };

  const logout = async () => {
    try {
      await api.post("auth_logout");
    } finally {
      setUser(null);
    }
  };

  const update = async (data: User): Promise<boolean> => {
    try {
      await api.put("auth_update", data);
      return true;
    } catch (err) {
      console.error("Update failed:", err);
      return false;
    }
  };

  const changePassword = async (data: PasswordChangeData): Promise<boolean> => {
    try {
      await api.put("auth_change_password", data);
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

    const interval = setInterval(refreshUser, 5 * 60 * 1000);

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