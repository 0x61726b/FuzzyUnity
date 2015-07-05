//.     .       .  .   . .   .   . .    +  .
//  .     .  :     .    .. :. .___---------___.
//       .  .   .    .  :.:. _".^ .^ ^.  '.. :"-_. .
//    .  :       .  .  .:../:            . .^  :.:\.
//        .   . :: +. :.:/: .   .    .        . . .:\
// .  :    .     . _ :::/:               .  ^ .  . .:\
//  .. . .   . - : :.:./.                        .  .:\
//  .      .     . :..|:                    .  .  ^. .:|
//    .       . : : ..||        .                . . !:|
//  .     . . . ::. ::\(                           . :)/
// .   .     : . : .:.|. ######              .#######::|
//  :.. .  :-  : .:  ::|.#######           ..########:|
// .  .  .  ..  .  .. :\ ########          :######## :/
//  .        .+ :: : -.:\ ########       . ########.:/
//    .  .+   . . . . :.:\. #######       #######..:/
//     :: . . . . ::.:..:.\           .   .   ..:/
//  .   .   .  .. :  -::::.\.       | |     . .:/
//     .  :  .  .  .-:.":.::.\             ..:/
// .      -.   . . . .: .:::.:.\.           .:/
//.   .   .  :      : ....::_:..:\   ___.  :/
//   .   .  .   .:. .. .  .: :.:.:\       :/
//     +   .   .   : . ::. :.:. .:.|\  .:/|
//     .         +   .  .  ...:: ..|  --.:|
//.      . . .   .  .  . ... :..:.."(  ..)"
// .   .       .      :  .   .: ::/  .  .::\


//      __       ___  ___  ___  ___      ___       ___      ___       __        ______    
//     /""\     |"  \/"  ||"  \/"  |    |"  |     |"  \    /"  |     /""\      /    " \   
//    /    \     \   \  /  \   \  /     ||  |      \   \  //   |    /    \    // ____  \  
//   /' /\  \     \\  \/    \\  \/      |:  |      /\\  \/.    |   /' /\  \  /  /    ) :) 
//  //  __'  \    /   /     /   /        \  |___  |: \.        |  //  __'  \(: (____/ //  
// /   /  \\  \  /   /     /   /        ( \_|:  \ |.  \    /:  | /   /  \\  \\        /   
//(___/    \___)|___/     |___/          \_______)|___|\__/|___|(___/    \___)\"_____/    
//--------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
//--------------------------------------------------------------------------------
public class EnemyCollider : MonoBehaviour
{
    //--------------------------------------------------------------------------------
    public AudioClip audio1;
    public AudioClip audio2;
    public AudioClip audio3;
    AudioSource audio;
    //--------------------------------------------------------------------------------
    private bool entered;
    private bool muted;
    private bool soundPlayed;
    //--------------------------------------------------------------------------------
    public FormationHandler fh;
    //--------------------------------------------------------------------------------
    private float timer;
    private int counter;
    private bool incremented;

    public void Start()
    {
        timer = 0;
        counter = 0;
        incremented = false;
        audio = GetComponent<AudioSource>();
        muted = false;
        entered = false;
        soundPlayed = false;

        if (PlayerPrefs.GetInt("Mute", 0) == 1)
        {
            MuteToggle();
        }
    }
    //--------------------------------------------------------------------------------
    public void Update()
    {
     
        if (entered)
        {
            timer += Time.deltaTime;
            if (timer > 2.5f)
            {
                entered = false;
                timer = 0f;
                counter = 0;
            }

        }

    }
    //--------------------------------------------------------------------------------
    public void OnTriggerEnter(Collider c)
    {
    
        entered = true;
       
            if (counter == 0)
            {
                if(!soundPlayed){
                    audio.clip = audio1;
                }
               
                counter = incremented ? counter : ++counter;
                incremented = true;
                
            }
            else if (counter == 1)
            {
                if (!soundPlayed)
                {
                    audio.clip = audio2;
                }
                counter = incremented ? counter : ++counter;
                incremented = true;
            }
            else if (counter == 2)
            {
                if (!soundPlayed)
                {
                    audio.clip = audio3;
                }
                counter = incremented ? counter : ++counter;
                incremented = true;
            }

            if (!soundPlayed)
            {
               
                audio.Play();
              
                soundPlayed = true;
                
            }
        
       

        fh.OnWaveCollision(c);
    }
    //--------------------------------------------------------------------------------
    public void OnTriggerExit(Collider c)
    {
        soundPlayed = false;
        incremented = false;
    }
    //--------------------------------------------------------------------------------

    public void MuteToggle()
    {
        audio.volume = muted ? 100 : 0;
        muted = !muted;
    }
}
//--------------------------------------------------------------------------------
