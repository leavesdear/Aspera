using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using System.IO;
public class TextPrinter : MonoBehaviour
{
    // 把按下Z键的判断换为检测fullText不为空







    public float typingSpeed = 0.05f; // 每个字符的显示间隔时间

    public static string fullText; // 完整的文本

    public static string currentText; // 显示的文本

    public static Text textComponent; // 文本组件



    public static AudioSource speaker;

    public AudioClip speak;



    public static bool canInput;

    public static bool stop;



    // 读取文件部分

    public string MyText;

    public int index;

    public static string pathName;



    void Start()

    {

        textComponent = GameObject.Find("对话").GetComponent<Text>();

        speaker = GameObject.Find("说话音效").GetComponent<AudioSource>();

        textComponent.text = "";

        fullText = "";

        canInput = true;

        stop = false;

    }




    void Update()

    {

        if (fullText != "" && canInput == true)

        {

            currentText = "";

            canInput = false;

            textComponent.text = "";

            // Debug.Log(fullText);

            StartCoroutine(TypeText());

        }



        // else if(Move_event.move_event == true)

        // {

        //     Move_event.move_event = false;

        //     canInput = false;

        //     textComponent.text = "";

        //     StartCoroutine(TypeText());



        // }



        //if (textComponent.text == "")

        //{

        //    UIManager_2.my_printer.SetActive(false);

        //}

        //else

        //{

        //    UIManager_2.my_printer.SetActive(true);

        //}



    }







    IEnumerator TypeText()

    {

        foreach (char c in fullText)

        {

            // switch (Character_new.ID)

            // {

            //     case "无":

            //     speaker.clip = speak;

            //     break;



            //     case "W":

            //     speaker.clip = speak_wolf;

            //     break;



            //     case "？？？":

            //     speaker.clip = speak_me;

            //     break;



            //     default:

            //     break;

            // }

            //if (Character_new.ID == "无")

            //{

            //    speaker.clip = speak;

            //}

            speaker.Play();

            currentText += c;

            textComponent.text = currentText; // 将显示文字传给文本组件

            yield return new WaitForSeconds(typingSpeed);

        }

        canInput = true;

        fullText = "";

        // if(Move_event.move_event == true)

        // {

        //     Move_event.move_event = false;

        // }

    }



    // string ReadFile(string PathName,int LineNumber)

    // {

    //     string[] strs = File.ReadAllLines(PathName);

    //     if(LineNumber == 0)

    //     {

    //         return "";

    //     }

    //     else

    //     {

    //         return strs[LineNumber - 1];

    //     }

    // }

}


