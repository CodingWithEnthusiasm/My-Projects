package com.example.project;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.NotificationCompat;
import java.text.SimpleDateFormat;
import java.util.Date;


public class RevertActivity extends AppCompatActivity {

    @Override

    protected void onCreate(Bundle savedInstanceState)
    {
        int id =123;
        super.onCreate(savedInstanceState);
        String date = new SimpleDateFormat("dd-MM-yyyy HH:mm:ss").format(new Date());

        int importance = NotificationManager.IMPORTANCE_HIGH;
        NotificationChannel notificationChannel = new NotificationChannel("4655", "NAME",importance );
        NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(this, "4655").setSmallIcon(R.drawable.divider).setContentTitle("RevertActivity").setContentText("OnCreate: "+ date);
        NotificationManager notificationManager =(NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
        notificationManager.notify(id, mBuilder.build());


        setContentView(R.layout.activity_revert);
        Intent intent = getIntent();
        String message = intent.getStringExtra(MainActivity.EXTRA_MESSAGE);
        message = new StringBuilder(message).reverse().toString();
        TextView textView = findViewById(R.id.textView3);
        textView.setText(message);


    }
    @Override
    protected void onDestroy() {
        int id = 321;
        super.onDestroy();
        String date = new SimpleDateFormat("dd-MM-yyyy HH:mm:ss").format(new Date());
        int importance = NotificationManager.IMPORTANCE_HIGH;
        NotificationChannel notification = new NotificationChannel("4655", "NAME",importance );
        NotificationCompat.Builder Builder = new NotificationCompat.Builder(this, "4655").setSmallIcon(R.drawable.divider).setContentTitle("RevertActivity").setContentText("OnDestroy: "+ date);
        NotificationManager notificationManager =(NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
        notificationManager.notify(id, Builder.build());

    }

    public void Back(View view) {
       finish();
    }


}