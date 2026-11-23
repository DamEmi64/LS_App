import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import path from "path";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => ({
    server: {
        host: "0.0.0.0",
        port: 8080,
        proxy: {
            "/backend": {
                target: "http://127.0.0.1:5144",
                changeOrigin: true,
                ws: true,
                rewrite: (path) => path.replace(/^\/backend/, ""),
            },
        },
    },
    plugins: [react()],
    resolve: {
        alias: {
            "@": path.resolve(__dirname, "./src"),
        },
    },
}));
