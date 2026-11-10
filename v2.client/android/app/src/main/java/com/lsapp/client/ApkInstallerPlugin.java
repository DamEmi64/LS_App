package com.lsapp.client;

import android.content.Intent;
import android.os.Build;
import android.net.Uri;
import android.provider.Settings;
import androidx.core.content.FileProvider;

import com.getcapacitor.Plugin;
import com.getcapacitor.PluginCall;
import com.getcapacitor.PluginMethod;
import com.getcapacitor.annotation.CapacitorPlugin;

import java.io.File;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;

@CapacitorPlugin(name = "ApkInstaller")
public class ApkInstallerPlugin extends Plugin {
    @PluginMethod
    public void downloadAndInstall(PluginCall call) {
        String url = call.getString("url");
        if (url == null || !url.startsWith("https://")) {
            call.reject("A valid HTTPS download URL is required");
            return;
        }

        new Thread(() -> download(call, url)).start();
    }

    private void download(PluginCall call, String address) {
        File apk = new File(getContext().getCacheDir(), "app-update.apk");
        HttpURLConnection connection = null;
        try {
            connection = (HttpURLConnection) new URL(address).openConnection();
            connection.setConnectTimeout(20000);
            connection.setReadTimeout(30000);
            connection.setInstanceFollowRedirects(true);
            connection.connect();
            int status = connection.getResponseCode();
            if (status != HttpURLConnection.HTTP_OK) throw new IllegalStateException("APK download failed (HTTP " + status + ")");
            long total = connection.getContentLengthLong();
            long received = 0;
            try (InputStream input = connection.getInputStream(); FileOutputStream output = new FileOutputStream(apk)) {
                byte[] buffer = new byte[64 * 1024];
                int count;
                while ((count = input.read(buffer)) != -1) {
                    output.write(buffer, 0, count);
                    received += count;
                    if (total > 0) notifyListeners("downloadProgress", new com.getcapacitor.JSObject().put("progress", (int) Math.min(99, received * 100 / total)));
                }
                output.flush();
            }
            if (received == 0) throw new IllegalStateException("The APK download was empty");

            Uri uri = FileProvider.getUriForFile(getContext(), getContext().getPackageName() + ".fileprovider", apk);
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O && !getContext().getPackageManager().canRequestPackageInstalls()) {
                Intent settings = new Intent(Settings.ACTION_MANAGE_UNKNOWN_APP_SOURCES, Uri.parse("package:" + getContext().getPackageName()));
                settings.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                getContext().startActivity(settings);
                call.reject("Allow installs from LS App in Android settings, then tap Download update again.");
                return;
            }
            Intent install = new Intent(Intent.ACTION_INSTALL_PACKAGE);
            install.setDataAndType(uri, "application/vnd.android.package-archive");
            install.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION | Intent.FLAG_ACTIVITY_NEW_TASK);
            getContext().startActivity(install);
            notifyListeners("downloadProgress", new com.getcapacitor.JSObject().put("progress", 100));
            call.resolve();
        } catch (Exception error) {
            apk.delete();
            String message = error.getMessage();
            call.reject(message == null ? "Could not download or open the APK installer" : message, error);
        } finally {
            if (connection != null) connection.disconnect();
        }
    }
}
