#include <Arduino.h>

#include <Bounce2.h>
Bounce2::Button but0;

#include <Chrono.h>
Chrono chronoPot;

#include <MicroOscSlip.h>
MicroOscSlip<128> monOsc(&Serial);

#define POTENTIOMETER_PIN A1

void setup()
{
    Serial.begin(115200);

    but0.attach(8, INPUT_PULLUP);
    but0.setPressedState(LOW);

    pinMode(5, OUTPUT);
}

void loop()
{
    but0.update();

    if (but0.pressed())
    {
        monOsc.sendInt("/bang", 1);
    }

    monOsc.onOscMessageReceived(traiterMessageOsc);

    if (chronoPot.hasPassed(25))
    {
        chronoPot.restart(); // REPARTIR LE CHRONO

        int valeur = analogRead(1); // LECTURE DE LA TENSION DE LA BROCHE A1 ENTRE 0 ET 1023

        monOsc.sendInt("/turn", valeur); // ENVOYER LA VALEUR
    }
}

// FONCTION APPELÉE LORSQU’UN MESSAGE OSC EST REÇU
void traiterMessageOsc(MicroOscMessage &message)
{
    if (message.checkOscAddressAndTypeTags("/ready", "i"))
    {

        int valeur = message.nextAsInt(); // RÉCUPÉRER L’ENTIER ("i")

        digitalWrite(5, valeur); // ASSIGNER LA VALEUR A LA DEL
    }
}
