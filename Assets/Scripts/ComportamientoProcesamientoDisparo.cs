/* 
* Copyright 2007 Carlos González Díaz
*  
* Licensed under the EUPL, Version 1.1 or – as soon they
will be approved by the European Commission - subsequent
versions of the EUPL (the "Licence");
* You may not use this work except in compliance with the
Licence.
* You may obtain a copy of the Licence at:
*  
*
https://joinup.ec.europa.eu/software/page/eupl
*  
* Unless required by applicable law or agreed to in
writing, software distributed under the Licence is
distributed on an "AS IS" basis,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
express or implied.
* See the Licence for the specific language governing
permissions and limitations under the Licence.
*/ 
using UnityEngine;
using System.Collections;
using System.IO.Ports; // Para usar la clase Serial Port

/// <summary>
/// This is the input from the arduino [NEEDS REFACTORING]
/// </summary>
public class ComportamientoProcesamientoDisparo : MonoBehaviour 
{
	
    /// The serial port to read
    [SerializeField]
    string SerialPortToRead;

    /// The readTimeOut
    [SerializeField]
    int portReadTimeOut;
        
    /// El puerto de serie que queremos abrir y leer
    SerialPort sp;
	
	/// El valor entero que recibimos del puerto de serie
	int valueReadInSP;

    /// The gameObject to send messages
    [SerializeField]
    GameObject objectToMessage;

    /// To maintain the couroutine alive
    bool couroutineToLive;

    /// Awake is called when the script instance is being loaded
    public void Awake()
    {
        //this.sp = new SerialPort(SerialPortToRead, 9600, Parity.None, 8, StopBits.One);
        //this.sp = new SerialPort("\\\\.\\COM10", 9600, Parity.None, 8, StopBits.One);
        this.sp = new SerialPort("\\\\.\\" + SerialPortToRead, 9600, Parity.None, 8, StopBits.One);
    }
	
	/// Use this for initialization
	void Start () 
	{
		couroutineToLive = true;

        StartPort();

        //StartCoroutine(PortCoroutine(sp));

        StartCoroutine("PortCoroutine", sp);
		
	}
	
	/// Update is called once per frame
	void Update () 
	{

        //UpdateLoop();
	}

    void StartPort()
    {
        // Abrimos el puerto de serie que hemos definido al principio de la clase
        try
        {
            sp.Open();
        }
        catch (System.Exception ex)
        {


            Debug.Log(ex.Message.ToString());
        }

        /* Evitamos que espere más de un segundo para leer un valor dentro del puerto 
         y la ejecución pueda continuar sin congelarse el simulador
        */
        sp.ReadTimeout = portReadTimeOut;
    }

    IEnumerator PortCoroutine(SerialPort sp)
    {
        int numOfRuns = 0;

        Debug.Log("Coroutine started! Times: " + numOfRuns.ToString());
        

        while (couroutineToLive)
        {
            try
            {
                UpdateLoop();
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning(ex.Message.ToString());
                
            }
            yield return null;
        }
        numOfRuns++;

        Debug.Log("Coroutine ended!");

        StopCoroutine("PortCoroutine");
        yield return null;
    }

    void UpdateLoop()
    {
        // Leemos el puerto de serie
        ReadSerialPort();
        // Disparamos una bala
        Shoot();
        // Reiniciamos el valor leido del puerto de serie 
        ResetValue();
    }
	
	/// La función que escucha en el puerto de serie y almacena el valor en una variable
	void ReadSerialPort () 
	{
		// Comprobamos que el puerto está abierto
		if (sp.IsOpen) 
		{
			// Iniciamos un try catch para evitar que la ejecución se interrumpa si salta una excepción
			try 
			{
                // Guardamos el valor leido en la variable entera que hemos definido arriba
                if (sp.ReadByte() == 1)
                {
                    Debug.Log("Leido 1!");
                    this.valueReadInSP = 1;
                }
                
				//print(this.valueReadInSP);
                Debug.Log("El valor leido es: " + valueReadInSP.ToString());
			}
			catch (System.Exception ex)
			{
                Debug.Log(ex.Message.ToString());
			}

		}

	}
	
	/// La función que se encargará de enviar la señal de disparo si el entero leido es el que queremos
	void Shoot () 
	{
		// Si el valor leido es 1
		if (valueReadInSP == 1) 
		{
			// Enviamos un mensaje al gestor de entradas del simulador para que dispare

            try
            {
                this.objectToMessage.SendMessage("CheckInputAndShoot");
            }
            catch (System.Exception ex)
            {

                Debug.Log(ex.Message.ToString());
            }

            
		}
	}
	
	/// La función que reinicia el valor entero de la clase para evitar que se lea incorrectamente
	void ResetValue () 
	{
		// Ponemos el valor a cero y evitamos enviar el mensaje de disparo por error en Shoot
		this.valueReadInSP = 0;
	}

    // Sent to all game objects before the application is quit
    public void OnApplicationQuit()
    {
        couroutineToLive = false;
        StopCoroutine("PortCoroutine");
    }


}