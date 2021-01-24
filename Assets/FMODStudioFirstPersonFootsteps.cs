
using UnityEngine;

public class FMODStudioFirstPersonFootsteps : MonoBehaviour
{
    
    [Header("FMOD Settings")]
    [SerializeField] [FMODUnity.EventRef] private string FootstepsEventPath;    
    [SerializeField] [FMODUnity.EventRef] private string JumpingEventPath;      
    [SerializeField] private string MaterialParameterName;                      
    [SerializeField] private string SpeedParameterName;                         
    [SerializeField] private string JumpOrLandParameterName;                    
    [Header("Playback Settings")]
    [SerializeField] private float StepDistance = 2.0f;                         
    [SerializeField] private float RayDistance = 1.2f;                          
    [SerializeField] private float StartRunningTime = 0.3f;                     
    [SerializeField] private string JumpInputName;                              
    public string[] MaterialTypes;                                              
    [HideInInspector] public int DefulatMaterialValue;                          


    
    private float StepRandom;                                                   
    private Vector3 PrevPos;                                                    
    private float DistanceTravelled;                                            
    
    private RaycastHit hit;                                                     
    private int F_MaterialValue;                                                
    
    private bool PlayerTouchingGround;                                          
    private bool PreviosulyTouchingGround;                                      
    
    private float TimeTakenSinceStep;                                           
    private int F_PlayerRunning;                                                


    void Start() 
    {
        StepRandom = Random.Range(0f, 0.5f);       
        PrevPos = transform.position;               
    }


    void Update() 
    {

        Debug.DrawRay(transform.position, Vector3.down * RayDistance, Color.blue);       


        
        GroundedCheck();                                                   
        if (PlayerTouchingGround && Input.GetButtonDown(JumpInputName))    
        {
            MaterialCheck();                                               
            PlayJumpOrLand(true);                                          
        }
        if (!PreviosulyTouchingGround && PlayerTouchingGround)             
        {
            MaterialCheck();                                               
            PlayJumpOrLand(false);                                        
        }
        PreviosulyTouchingGround = PlayerTouchingGround; 


        
        TimeTakenSinceStep += Time.deltaTime;                                
        DistanceTravelled += (transform.position - PrevPos).magnitude;       
        if (DistanceTravelled >= StepDistance + StepRandom)                  
        {
            MaterialCheck();                                                 
            SpeedCheck();                                                    
            PlayFootstep();                                                  
            StepRandom = Random.Range(0f, 0.5f);                             
            DistanceTravelled = 0f;                                          
        }
        PrevPos = transform.position;                                        

    }


    void MaterialCheck() 
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, RayDistance))                                 
        {
            if (hit.collider.gameObject.GetComponent<FMODStudioMaterialSetter>())                                    
            {
                F_MaterialValue = hit.collider.gameObject.GetComponent<FMODStudioMaterialSetter>().MaterialValue;    
            }
            else                                                                                                     
                F_MaterialValue = DefulatMaterialValue;                                                              
        }
        else                                                                                                         
            F_MaterialValue = DefulatMaterialValue;                                                                  
    }


    void SpeedCheck() 
    {
        if (TimeTakenSinceStep < StartRunningTime)                     
            F_PlayerRunning = 1;                                       
        else                                                           
            F_PlayerRunning = 0;                                       
        TimeTakenSinceStep = 0f;                                       
    }


    void GroundedCheck()
    {
        Physics.Raycast(transform.position, Vector3.down, out hit, RayDistance);
        if (hit.collider)                                                           
            PlayerTouchingGround = true;                                            
        else                                                                        
            PlayerTouchingGround = false;                                          
    }


    void PlayFootstep() 
    {
        if (PlayerTouchingGround)                                                                                    
        {
            FMOD.Studio.EventInstance Footstep = FMODUnity.RuntimeManager.CreateInstance(FootstepsEventPath);       
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(Footstep, transform, GetComponent<Rigidbody>());     
            Footstep.setParameterByName(MaterialParameterName, F_MaterialValue);                                     
            Footstep.setParameterByName(SpeedParameterName, F_PlayerRunning);                                        
            Footstep.start();                                                                                       
            Footstep.release();                                                                                   
        }
    }


    void PlayJumpOrLand(bool F_JumpLandCalc) 
    {
        FMOD.Studio.EventInstance Jl = FMODUnity.RuntimeManager.CreateInstance(JumpingEventPath);         
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(Jl, transform, GetComponent<Rigidbody>());  
        Jl.setParameterByName(MaterialParameterName, F_MaterialValue);                                    
        Jl.setParameterByName(JumpOrLandParameterName, F_JumpLandCalc ? 0f : 1f);                        
        Jl.start();                                                                                       
        Jl.release();                                                                                    
    }
}

