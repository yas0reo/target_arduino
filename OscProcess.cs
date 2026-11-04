using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;


public class OscProcess : MonoBehaviour
{
    public OSCReceiver oscReceiver;
    public OSCTransmitter oscTransmitter;
    public Player Player;

    public int betaInMin = 0;
    public int betaInMax = 1023;
    public float betaOutMin = 0.0f;
    public float betaOutMax = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        oscReceiver.Bind("/bang", TraiterMessageBang);
        oscReceiver.Bind("/turn", TraiterMessageTurn);

    }

    // Update is called once per frame
    void Update()
    {
        if (Player.getCanFire())
        {
            var oscMessage = new OSCMessage("/ready");
            oscMessage.AddValue(OSCValue.Int(1)); // Ajoute l’entier 1
            oscTransmitter.Send(oscMessage); // Envoyer le message 
        }
        else
        {
            var oscMessage = new OSCMessage("/ready");
            oscMessage.AddValue(OSCValue.Int(0)); // Ajoute l’entier 0
            oscTransmitter.Send(oscMessage); // Envoyer le message 
        }
    }

    void TraiterMessageBang(OSCMessage message)
    {
        int valeur = message.Values[0].IntValue;

        // Traiter la valeur
        if (valeur == 1)
        {
            // Si nécéssaire, appeler une fonction ici
            Player.Shoot();
        }
        else
        {
            // Si nécéssaire, appeler une fonction ici
        }

    }
    void TraiterMessageTurn(OSCMessage message)
    {
        // Traiter un entier et l’appliquer à une propriété (flux continu)

        // Récupérer la valeur du premier argument en taant qu’entier :
        int valeur = message.Values[0].IntValue;

        // Deboguer
        // Debug.Log("Reçu : " + message.Address + " " + valeur);

        // Ajustement de la valeur
        float ajustee = (
            ((float)valeur - betaInMin) / (betaInMax - betaInMin) * (betaOutMax - betaOutMin) + betaOutMin
        );

        // DESTINATION
        Player.SetPlayerRotation(ajustee);

    }


}

