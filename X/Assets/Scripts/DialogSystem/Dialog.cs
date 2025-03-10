using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField] private Type DialogType;
        [SerializeField] private DialogStruct[] dialog;
        [SerializeField] private bool isSkipeble;
        [SerializeField] private bool isCoolDown;


        private DialogWindow currentDialogWindow;
        private bool dialogResume = false, isDialogStart = false;
        private int dialogNumber = 0;

        private PlayerMovement character;

        [System.Serializable]
        public struct DialogStruct
        {
            public string text;
            public float dialogTime;
            public Transform dialogObjectTransform;
            public UnityEvent DialogEvent;
        }
        public enum Type
        {
            Trigger,
            Invoke,
            ButtonInput
        }
        public void StartDialog()
        {
            character = FindAnyObjectByType<PlayerMovement>();

            if (isDialogStart == false)
            {
                dialogResume = true;
                isDialogStart = true;
                InstantiateDialogMessege();

                if (isCoolDown)
                {
                    FlipCharacterIfCoolDown();
                }
            }         
        }
        public void SkipDialogMessege()
        {

        }
        public void EndDialog()
        {
            dialogResume =  false;
            dialogNumber = dialog.Length;
            DestroyLastMessege();

            if (isCoolDown) FindAnyObjectByType<PlayerMovement>().coolDown = false;
        }
        private IEnumerator NextMessegeCoolDown(float coolDownTime)
        {
            yield return new WaitForSeconds(coolDownTime);
            if (currentDialogWindow != null) currentDialogWindow.GetComponent<Animator>().Play("Fade");
            yield return new WaitForSeconds(1f);
            dialogResume = true;
        }
        private IEnumerator NextSkipMessegeCoolDown(float coolDownTime)
        {
            yield return new WaitForSeconds(coolDownTime);
            dialogResume = true;
        }
        private void Update()
        {
            if (!isSkipeble)
            {
                if (dialogResume) 
                {
                    if (dialogNumber <= dialog.Length - 1)
                    {
                        StartCoroutine(NextMessegeCoolDown(dialog[dialogNumber].dialogTime));
                        DestroyLastMessege();
                        InstantiateDialogMessege();

                        dialog[dialogNumber].DialogEvent.Invoke();

                        dialogNumber++;

                        dialogResume = false;            
                    }
                    else
                    {
                        EndDialog();
                    }
                }             
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E) && dialogResume)
                { 
                    if(dialogNumber <= dialog.Length - 2)
                    {
                        dialogNumber++;
                        StartCoroutine(NextSkipMessegeCoolDown(0.1f));
                        DestroyLastMessege();
                        InstantiateDialogMessege();

                        dialog[dialogNumber].DialogEvent.Invoke();
                    }
                    else
                    {
                        EndDialog();
                    }
                }            
            }       
        }
        private void FlipCharacterIfCoolDown()
        {
            character.coolDown = true;

            if (character.transform.position.x > dialog[dialogNumber].dialogObjectTransform.position.x && character.facingRight)
            {
                character.Flip();
            }
            if (character.transform.position.x < dialog[dialogNumber].dialogObjectTransform.position.x && !character.facingRight)
            {
                character.Flip();
            }
        }
        private void InstantiateDialogMessege()
        {
            if(dialogNumber <= dialog.Length - 1)
            {
                if (dialog[dialogNumber].text.Length >= 10 && dialog[dialogNumber].text.Length <= 18)
                {
                    currentDialogWindow = Instantiate(Resources.Load<DialogSystem.DialogWindow>("DialogSystem/dialogWindow_medium"));
                }
                else if (dialog[dialogNumber].text.Length >= 18)
                {
                    currentDialogWindow = Instantiate(Resources.Load<DialogSystem.DialogWindow>("DialogSystem/dialogWindow_large"));
                }
                else
                {
                    currentDialogWindow = Instantiate(Resources.Load<DialogSystem.DialogWindow>("DialogSystem/dialogWindow_small")); 
                }


                currentDialogWindow.transform.position = dialog[dialogNumber].dialogObjectTransform.position;
                currentDialogWindow.transform.position = new Vector3(currentDialogWindow.transform.position.x, currentDialogWindow.transform.position.y, -9f);
                currentDialogWindow.transform.position = new Vector2(currentDialogWindow.transform.position.x, transform.position.y + (dialog[dialogNumber].dialogObjectTransform.GetComponent<SpriteRenderer>().bounds.size.y / 2));
                currentDialogWindow.GetComponent<Animator>().Play("UnFade");
                currentDialogWindow.SetText(dialog[dialogNumber].text);
            }          
        }
        private void DestroyLastMessege()
        {
            if (currentDialogWindow != null)
            {
                Destroy(currentDialogWindow.gameObject);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player" && DialogType == Type.Trigger)
            {
                if(!isDialogStart) StartDialog();
            }
        }
    }
}
