package com.example.project;
import androidx.activity.result.ActivityResultLauncher;
import androidx.activity.result.contract.ActivityResultContracts;
import androidx.appcompat.app.AppCompatActivity;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Chronometer;
import android.widget.EditText;
import android.widget.TextView;

import java.util.Arrays;


public class MainActivity extends AppCompatActivity {



    ActivityResultLauncher<Intent> launcher = registerForActivityResult(
            new ActivityResultContracts.StartActivityForResult(),
            result -> {
                if (result.getResultCode()== Activity.RESULT_OK)
                {
                    Intent data = result.getData();
                    if(data!=null)
                        handleResult(data);
                }
            }
    );
    Chronometer chronometer;

    public static final String EXTRA_MESSAGE = "com.example.project.MESSAGE";
    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        chronometer = findViewById(R.id.chron);
        chronometer.start();

    }
    private void handleResult(Intent result)
    {
        TextView OperationDisplay = findViewById(R.id.OperationDisplay);
        TextView ResultDisplay = findViewById(R.id.ResultDisplay);
        OperationDisplay.setText(result.getStringExtra(Intent.EXTRA_TEXT));
        ResultDisplay.setText(String.valueOf(result.getIntExtra("result", 0)));
    }


    public void Run(View view)
    {


        Intent intent = new Intent();
        Intent chooser =Intent.createChooser(intent, Arrays.toString(gatherData()));
        intent.putExtra(Intent.EXTRA_TEXT, gatherData());
        intent.setAction("operation");
        intent.setType("text/plain");

        //startActivity(chooser);
        launcher.launch(intent);


    }



    public void sendMessage(View view) {
        Intent intent = new Intent(this, RevertActivity.class);
        EditText editText = (EditText) findViewById(R.id.editTextTextPersonName);
        String message = editText.getText().toString();
        intent.putExtra(EXTRA_MESSAGE, message);
        startActivity(intent);

        }



    private int[] gatherData() {
        TextView arg1 = findViewById(R.id.arg1);
        TextView arg2 = findViewById(R.id.arg2);
        return new int[]
                {
                        getNumber(arg1),
                        getNumber(arg2)
                };
    }

    private int getNumber(TextView tv)
   {
      return getNumber(tv.getText().toString());


   }

    private int getNumber(String txt)
    {
        return Integer.parseInt(nullCheck(txt));
    }


    private String nullCheck(String txt)
    {
        return txt.equals("") ? "0" :txt;
    }

}

