//At the equator for longitude and for latitude anywhere, the following approximations are valid:
//1° = 111 km(or 60 nautical miles)
//0.1° = 11.1 km
//0.01° = 1.11 km(2 decimals, km accuracy)
//0.001° = 111 m
//0.0001° = 11.1 m
//0.00001° = 1.11 m 
//0.000001° = 0.11 m(7 decimals, cm accuracy)

//Earth is a sphere with a circumference of 40075 km == 40075000 m == 4007500000 cm
//Length in meters of 1° of latitude = always 111.32 km = 40075 km / 360
//Length in meters of 1° of longitude = 40075 km * cos( latitude ) / 360
// lat 1 m = 0.000008983156581 degrees

// lon 1 m = 0.000006273659336 degrees = cos(33.312090)*lat  = 0.8356914933 (QIRYAT-SHMONA)
// lon 1 m = 0.000007678791061 degrees = cos(31.2624908)*lat = 0.8547987549 (BEER-SHEVA)
// lon 1 m = 0.000007813607031 degrees = cos(29.563851)*lat  = 0.8698063938 (EILAT)
// blu blu
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;



public class SpinCity : MonoBehaviour
{

    List<InputDevice> devices;
    public Text compassText;
    public Text GPSText;
    public Text stat;
    public Text AndroidGPSText;
    public Camera cam;

    private Queue<int> qCompass;
    private Queue<Tuple<double, double>> qUnityGPSOrigin;
    private Queue<Tuple<double, double>> qAndroidGPSOrigin;
    private Queue<Tuple<double, double>> qAndroidGPSCam;
    public static readonly double EarthRadius = 6378.1; //#Radius of the Earth km
    private int unityCounter;
    private int androidCounter;
    private double lastCompassTimeStamp;
    private double lastUnityGPSTimeStamp = 0;
    private long lastAndroidGPSTimeStamp = 0;
    public static int toRotate;
    private float avgCompass;
    AndroidJavaObject androidCompassProvider;
    long androidCompassTime;
    AndroidJavaObject gpsProvider;
    double camDistFromOrigin;
    double headingFromCamToOrigin;
    private void Awake()
    {
        if (!Input.location.isEnabledByUser) //FIRST IM CHACKING FOR PERMISSION IF "true" IT MEANS USER GAVED PERMISSION FOR USING LOCATION INFORMATION
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }

    private void Start()
    {
        Input.compass.enabled = true;
        Input.location.Start(0.000000f,0.000000f);
        qCompass = new Queue<int>();
        qUnityGPSOrigin = new Queue<Tuple<double, double>>();
        qAndroidGPSOrigin = new Queue<Tuple<double, double>>();
        qAndroidGPSCam = new Queue<Tuple<double, double>>();
        unityCounter = 0;
        androidCounter = 0;
        
        // initialize the time stamps(to be compared in the first gps read)
        lastCompassTimeStamp = Input.compass.timestamp;
        lastUnityGPSTimeStamp = Input.location.lastData.timestamp;

        // create the current UNITY activity
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //instantiate the android plugin
        gpsProvider = new AndroidJavaObject("com.example.gpsplugin.GPSProvider", unityActivity);

        //instantiate the android plugin
        androidCompassProvider = new AndroidJavaObject("com.example.compasslib.CompassLib", unityActivity);
        androidCompassTime = 0;
    }
    // Update is called once per frame
    void Update()
    {
        stat.text = "state: " + ARSession.state;

        // dump compass and GPS data when tracking is lost
        if (ARSession.state != ARSessionState.SessionTracking)
        {
            // reset the compass data
            qCompass.Clear();
            lastCompassTimeStamp = Input.compass.timestamp;

            // reset the GPS data
            unityCounter = 0;
            androidCounter = 0;
            File.Delete(Application.persistentDataPath + "/compass.txt");

            File.Delete(Application.persistentDataPath + "/camlat.txt");
            File.Delete(Application.persistentDataPath + "/camlon.txt");

            File.Delete(Application.persistentDataPath + "/Androidcamlat.txt");
            File.Delete(Application.persistentDataPath + "/Androidcam_x.txt");
            File.Delete(Application.persistentDataPath + "/Androidcam_z.txt");
            File.Delete(Application.persistentDataPath + "/Androidcam_ARheading.txt");
            File.Delete(Application.persistentDataPath + "/Androidcam_heading.txt");
            File.Delete(Application.persistentDataPath + "/unified.txt");
            
            File.Delete(Application.persistentDataPath + "/Androidcamlon.txt");
            File.Delete(Application.persistentDataPath + "/Androidcamdist.txt");

            File.Delete(Application.persistentDataPath + "/AndroidcamlatAVG.txt");
            File.Delete(Application.persistentDataPath + "/AndroidcamlonAVG.txt");
            File.Delete(Application.persistentDataPath + "/AndroidoriginlatAVG.txt");
            File.Delete(Application.persistentDataPath + "/AndroidoriginlonAVG.txt");
            File.Delete(Application.persistentDataPath + "/Androidoriginlat.txt");
            File.Delete(Application.persistentDataPath + "/Androidoriginlon.txt");

            File.Delete(Application.persistentDataPath + "/Android_gps_calc.txt");
            
        }
        else// AR is tracking:
        {
            // HEADING

            // add a new compass read if exist
            AddCompassRead();
            
            // rotate the virtual city to align with the readings
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -avgCompass, transform.eulerAngles.z);
            //compassText.text = "" + (float)qCompass.Count / 10.0f + "%";
            
            //GPS

            //AddUnityGPSRead();

            AddAndroidGPSRead();
        }

    }
    // add to the queue only if there is a new compass reading  
    void AddCompassRead()
    {
        long act = androidCompassProvider.Get<long>("time");
        // check if the compass reading at this frame has a new timestamp
        if (act > androidCompassTime)
        {
            androidCompassTime = act;

            int heading = androidCompassProvider.Get<int>("azimuth");
            //int camY = (int)cam.transform.eulerAngles.y;
            int camY = CapMove.yRotAR;
            int camParentY = (int)cam.transform.rotation.eulerAngles.y;

            // add the new compass reading to the queue
            int hmc = heading - camY;
            toRotate = (heading + 360 - camY) % 360;
            qCompass.Enqueue(toRotate);

            // maintain the vector to be of a 1000 samples
            //if (qCompass.Count > 1000)
            //    qCompass.Dequeue();

            // update the last time a compass was read
            //lastCompassTimeStamp = Input.compass.timestamp;

            // get respective average of all compass readings
            avgCompass = getCompassAvg(qCompass);

            //File.AppendAllText(Application.persistentDataPath + "/compass.txt", "time: " + lastCompassTimeStamp + "\n");
            compassText.text = "heading: " + heading +
                "\ncamY: " + camY +
                "\ncamParentY: " + camParentY +
                "\nheading - camy: " + hmc +
                "\nheading - camy+360: " + (hmc + 360) +
                "\n(heading-camy+360) %360: " + toRotate +
                "\navg: " + avgCompass;

        }
    }
    private void AddAndroidGPSRead()
    {
        long time = gpsProvider.Get<long>("time");
        //bool availability = gpsProvider.Get<bool>("availability");

        if ( time > lastAndroidGPSTimeStamp)
        {
            Debug.Log("talikar time is bigger");

            androidCounter++;
            //AndroidGPSText.text = "counter: " + androidCounter;
            lastAndroidGPSTimeStamp = time;
            // calculate new lat-lon for the origin 
            double lat = gpsProvider.Get<double>("lat");
            double lon = gpsProvider.Get<double>("lon");

            //Log the gps coordinates
            //File.AppendAllText(Application.persistentDataPath + "/Androidcamlat.txt", "lat: " + lat + "\n");
            //File.AppendAllText(Application.persistentDataPath + "/Androidcamlon.txt", "lon: " + lon + "\n");
            //File.AppendAllText(Application.persistentDataPath + "/unified.txt", "lat read: " + lat + "\n");
            //File.AppendAllText(Application.persistentDataPath + "/unified.txt", "lon read: " + lon + "\n");
            AndroidGPSText.text = "count: " + androidCounter;
            AndroidGPSText.text += "\nlat: " + lat;
            AndroidGPSText.text += "\nlon: " + lon;

            // camera gps (USE ONLY WHEN CAMERA IS STATIC)
            //qAndroidGPSCam.Enqueue(new Tuple<double, double>(lat, lon));
            //Tuple<double, double> avgGPSCam = getGPSAvg(qAndroidGPSCam);

            //Log the gps coordinates
            //File.AppendAllText(Application.persistentDataPath + "/AndroidcamlatAVG.txt", "lat: " + avgGPSCam.Item1 + "\n");
            //File.AppendAllText(Application.persistentDataPath + "/AndroidcamlonAVG.txt", "lon: " + avgGPSCam.Item2 + "\n");

            // calculate origin of coordinate system
            Tuple<double, double> tup = GetOriginLatLon(lat, lon);
            //File.AppendAllText(Application.persistentDataPath + "/Androidoriginlat.txt", "lat: " + tup.Item1 + "\n");
            //File.AppendAllText(Application.persistentDataPath + "/Androidoriginlon.txt", "lon: " + tup.Item2 + "\n");

            // add the calculated lat-lon of the origin to the vector
            qAndroidGPSOrigin.Enqueue(tup);

            // get the average origin lat-lon in real world coordinates
            Tuple<double, double> avgGPSOrigin = getGPSAvg(qAndroidGPSOrigin);
            //File.AppendAllText(Application.persistentDataPath + "/AndroidoriginlatAVG.txt", "lat: " + avgGPSOrigin.Item1 + "\n");
            //File.AppendAllText(Application.persistentDataPath + "/AndroidoriginlonAVG.txt", "lon: " + avgGPSOrigin.Item2 + "\n");

            //Debug.Log("talikar avg...");
            AndroidGPSText.text += "\navg orig lat: " + avgGPSOrigin.Item1;
            AndroidGPSText.text += "\navg orig lon: " + avgGPSOrigin.Item2;
            //"\ncounter: " + androidCounter;// +
            //"\norigin-lat: \n" + avgGPSOrigin.Item1 +
            //"\norigin-lon: \n" + avgGPSOrigin.Item2;

        }
    }
    // get the latitude and longitude for the origin
    Tuple<double, double> GetOriginLatLon(double phoneLat, double phoneLon)
    {
        //File.AppendAllText(Application.persistentDataPath + "/Androidcam_x.txt", "x: " + cam.transform.position.x + "\n");
        //File.AppendAllText(Application.persistentDataPath + "/Androidcam_z.txt", "z: " + cam.transform.position.z + "\n");
        //File.AppendAllText(Application.persistentDataPath + "/unified.txt", "ar x: " + cam.transform.position.x + "\n");
        //File.AppendAllText(Application.persistentDataPath + "/unified.txt", "ar z: " + cam.transform.position.z + "\n");
        AndroidGPSText.text += "\nar x: " + cam.transform.position.x;
        AndroidGPSText.text += "\nar z: " + cam.transform.position.z;

        // calculate the distance between the camera and the origin
        camDistFromOrigin = Math.Sqrt(Math.Pow(cam.transform.position.x, 2) + Math.Pow(cam.transform.position.z, 2));

        //File.AppendAllText(Application.persistentDataPath + "/Androidcamdist.txt", "dist: " + camDistFromOrigin + "\n");
        //File.AppendAllText(Application.persistentDataPath + "/unified.txt", "ar dist: " + camDistFromOrigin + "\n");
        AndroidGPSText.text += "\nar dist: " + camDistFromOrigin;
        
        // calculate the heading between the current camera position and the origin in world coordinate system
        headingFromCamToOrigin = getHeadingToOriginInRealWorldCoordinateSystem();
        //File.AppendAllText(Application.persistentDataPath + "/Androidcam_heading.txt", "heading: " + headingFromCamToOrigin + "\n");
        //File.AppendAllText(Application.persistentDataPath + "/unified.txt", "real heading: " + headingFromCamToOrigin + "\n");
        AndroidGPSText.text += "\nreal heading: " + headingFromCamToOrigin;

        // calculate new lat-lon for the origin 
        //Tuple<double, double> orig = CalculateOriginLatLon(phoneLat, phoneLon, headingFromCamToOrigin, camDistFromOrigin);

        double east = camDistFromOrigin* Math.Sin(headingFromCamToOrigin.ToRadians());
        double north = camDistFromOrigin* Math.Cos(headingFromCamToOrigin.ToRadians());
        AndroidGPSText.text += "\neast: " + east;
        AndroidGPSText.text += "\nnorth: " + north;


        double latMeterAngle = 0.000008983156581;
        double lonMeterAngle = 0.000008983156581*Math.Cos(phoneLat.ToRadians());
        double origLat = phoneLat + north * latMeterAngle;
        double origLon = phoneLon + east * lonMeterAngle;
        AndroidGPSText.text += "\n origin lat: " + origLat;
        AndroidGPSText.text += "\n origin lon: " + origLon;

        Tuple<double, double> orig = new Tuple<double, double>(origLat, origLon);

        //File.AppendAllText(Application.persistentDataPath + "/Android_gps_calc.txt", "lat: " + phoneLat + "\n" +
        //                                                                            "lon: " + phoneLon + "\n" +
        //                                                                            "dist: " + camDistFromOrigin + "\n" +
        //                                                                            "heading: " + headingFromCamToOrigin + "\n" +
        //                                                                            "east: " + east + "\n" +
        //                                                                            "north: " + north + "\n") ;

        return orig;
    }
    public Tuple<double, double> CalculateOriginLatLon(double fmLat, double fmLon, double heading, double distanceKm)
    {

        double bearingR = heading.ToRadians();

        double latR = fmLat.ToRadians();
        double lonR = fmLon.ToRadians();

        double distanceToRadius = distanceKm / EarthRadius;

        double newLatR = Math.Asin(Math.Sin(latR) * Math.Cos(distanceToRadius)
                        + Math.Cos(latR) * Math.Sin(distanceToRadius) * Math.Cos(bearingR));

        double newLonR = lonR + Math.Atan2(
                                            Math.Sin(bearingR) * Math.Sin(distanceToRadius) * Math.Cos(latR),
                                            Math.Cos(distanceToRadius) - Math.Sin(latR) * Math.Sin(newLatR)
                                           );

        return new Tuple<double, double>(newLatR.ToDegrees(), newLonR.ToDegrees());

    }
    // get the heading between true north and origin(0,0) when the GPS coordinate is the axis
    double getHeadingToOriginInRealWorldCoordinateSystem()
    {
        // get the angle on the y axis (when z axis is 0 degrees) to the origin
        double AngleToOriginInARcoordinateSystem = getAngleToOriginInARcoordinateSystem();
        //File.AppendAllText(Application.persistentDataPath + "/Androidcam_ARheading.txt", "heading: " + AngleToOriginInARcoordinateSystem + "\n");
        //File.AppendAllText(Application.persistentDataPath + "/unified.txt", "ar heading: " + AngleToOriginInARcoordinateSystem + "\n");
        AndroidGPSText.text += "\nar heading: " + AngleToOriginInARcoordinateSystem;

        // the CW angle the coordinate system needs to rotate to be aligned with true North 
        double diff1 = 360.0 - avgCompass;
        // get the CW angle in the true north coordinate system between GPS and origin
        double diff2 = AngleToOriginInARcoordinateSystem - diff1;
        // normalize to positive
        double diff3 = (360 + diff2) % 360.0;
        return diff3;
    }

    // this angle is based on position of the camera with respect to origin of the AR coordinate system
    // (NOTICE: it has nothing to do with rotation of the camera!)
    // return value is angle in degrees in the range of 0.0 to 360.0
    double getAngleToOriginInARcoordinateSystem()
    {
        double angle = Math.Atan2(-cam.transform.position.x, -cam.transform.position.z);
        angle = angle.ToDegrees();
        // normalize to range 0 - 360
        angle = ((angle + 360.0) % 360.0);
        return angle;
    }

    // collect additional GPS sensor reading for calculation
    void AddUnityGPSRead()
    {
        if (Input.location.status != LocationServiceStatus.Running)
            GPSText.text = "" + Input.location.status;
        else
        {
            if (Input.location.lastData.timestamp > lastUnityGPSTimeStamp)
            {
                unityCounter++;
                // calculate new lat-lon for the origin 
                Tuple<double, double> tup = GetOriginLatLon(Input.location.lastData.latitude, Input.location.lastData.longitude);

                // add the calculated lat-lon of the origin to the vector
                qUnityGPSOrigin.Enqueue(tup);
                // get the average origin lat-lon in real world coordinates
                Tuple<double, double> avgGPS = getGPSAvg(qUnityGPSOrigin);
                
                //GPSText.text = "U LAT:" + Input.location.lastData.latitude +
                //    "\nU LON:" + Input.location.lastData.longitude +
                //    "\nA LAT:" + gpsProvider.Get<double>("lat") +
                //    "\nA LON:" + gpsProvider.Get<double>("lon") +
                //    "\ncounter: " + unityCounter +
                //    //"\ndist: " + camDistFromOrigin +
                //    //"\nheading: " + headingFromCamToOrigin+
                //    "\norigin-lat: \n" + avgGPS.Item1+
                //    "\norigin-lon: \n" + avgGPS.Item2;
                      

                lastUnityGPSTimeStamp = Input.location.lastData.timestamp;
            }
        }
    }

    
    public static float getCompassAvg(Queue<int> q)
    {
        float sum = 0;
        foreach (int f in q)
        {
            sum += f;
        }
        sum /= q.Count;
        return sum;
    }
    Tuple<double, double> getGPSAvg(Queue<Tuple<double, double>> qGPS)
    {
        double sumLat = 0;
        double sumLon = 0;
        foreach(Tuple<double, double> t in qGPS)
        {
            sumLat += t.Item1;
            sumLon += t.Item2;
        }
        sumLat=sumLat/ qGPS.Count;
        sumLon=sumLon/ qGPS.Count;
        return new Tuple<double, double>(sumLat,sumLon);
    }
    
}
public static class NumericExtensions
{
    public static double ToRadians(this double degrees)
    {
        return (Math.PI / 180) * degrees;
    }
    public static double ToDegrees(this double radians)
    {
        return (180 / Math.PI) * radians;
    }
    public static void SetAvg(this double d, int count)
    {
        d = d / count;
    }
}