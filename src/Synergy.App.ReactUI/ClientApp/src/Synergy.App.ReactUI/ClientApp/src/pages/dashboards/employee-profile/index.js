// ** React Imports
import { Fragment, useState, useEffect } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import RadioGroup from '@mui/material/RadioGroup'
import FormControlLabel from '@mui/material/FormControlLabel'
import Radio from '@mui/material/Radio'

import Step from '@mui/material/Step'
import Grid from '@mui/material/Grid'
import Button from '@mui/material/Button'
import Select from '@mui/material/Select'
import Divider from '@mui/material/Divider'
import Stepper from '@mui/material/Stepper'
import MenuItem from '@mui/material/MenuItem'
import StepLabel from '@mui/material/StepLabel'
import TextField from '@mui/material/TextField'
import Typography from '@mui/material/Typography'
import InputLabel from '@mui/material/InputLabel'
import IconButton from '@mui/material/IconButton'
import CardContent from '@mui/material/CardContent'
import FormControl from '@mui/material/FormControl'
import OutlinedInput from '@mui/material/OutlinedInput'
import FormHelperText from '@mui/material/FormHelperText'
import InputAdornment from '@mui/material/InputAdornment'

// ** Third Party Imports
import * as yup from 'yup'
import toast from 'react-hot-toast'
import { useForm, Controller } from 'react-hook-form'
import { yupResolver } from '@hookform/resolvers/yup'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Custom Components Imports
//import StepperCustomDot from './StepperCustomDot'

// ** Styled Components
import StepperWrapper from 'src/@core/styles/mui/stepper'

// import CustomInput

// ** Mytable Components
//import Mytable from './Mytable'
//import Buttonm from "./Buttonm"

//----------------------------------axios--------------------------
import axios from 'axios'
// import payroll 
//import Rayrolll from './Rayrolll'

// import payslip
//import Rayslip from './Rayslip'
import { display } from '@mui/system'



function createData(PersonNo, PersonFullName, Title, Gender, NationalityName, Religion, DateOfBirth, Status, Email, Mobile) {
  return { PersonNo, PersonFullName, Title, Gender, MaritalStatus, NationalityName, Religion, DateOfBirth, Status, Email, Mobile };
}
function creteData(DepartmentName,Title, AssignmentGradeId,id, UserId, DateOfBirth,PersonStatus, AssignmentTypeId, PositionId, Status, IsPrimaryAssignment, AnnualLeaveEntitlement,) {
  return { DepartmentName,Title, AssignmentGradeId, UserId,id, PersonStatus,DateOfBirth, AssignmentTypeId, PositionId, Status, IsPrimaryAssignment, AnnualLeaveEntitlement };
}


const steps = [
  {
    title: 'Personal Info',
    subtitle: ''
  },
  {
    title: 'Assingment',
    subtitle: 'Information'
  },
  {
    title: 'Contact',
    subtitle: 'Contact'
  }
  ,
  {
    title: 'Leave',
    subtitle: 'Leave'
  },
  {
    title: 'Attendance',
    subtitle: 'Attendance'
  },
  {
    title: 'Document',
    subtitle: 'Document'
  }
  ,
  {
    title: 'Depertment',
    subtitle: ' Depertment'
  },
  {
    title: 'PayRoll',
    subtitle: ' PayRoll'
  },
  {
    title: 'PaySlip',
    subtitle: ' PaySlip'
  },
]
const defaultAccountValues = {
  email: '',
  username: '',
  password: '',
  'confirm-password': ''
}

const defaultPersonalValues = {
  country: '',
  language: [],
  'last-name': '',
  'first-name': '',
  'Position': '',
  'Location': '',
  'Probation-Period': '',
  'Notice-Period': '',
  'Date-Of-Join': '',
}

const defaultSocialValues = {
  google: '',
  twitter: '',
  facebook: '',
  linkedIn: ''
}

const accountSchema = yup.object().shape({
  username: yup.string()

})

const personalSchema = yup.object().shape({
  country: yup.string(),

})

const socialSchema = yup.object().shape({

})

const StepperLinearWithValidation = () => {
  // ** States
const [activeStep, setActiveStep] = useState(0)

  const [state, setState] = useState({
    password: '',
    password2: '',
    showPassword: false,
    showPassword2: false

  })



  //API
  const [data, setUserdata] = useState({});

  useEffect(()=>{
    let userProfile = localStorage.getItem("userProfile");
    // console.log("Hello ", JSON.parse(userProfile));
    setUserdata(JSON.parse(userProfile));
    
//   console.log("Hello pooja");
    
  }, []);
  console.log("from localstorage ", data);
  console.log("============")
  
//   useEffect(() => {
//     axios.get('https://webapidev.aitalkx.com/chr/hrdirect/EmployeeProfile?userId=76e33a87-1e40-4767-9fc4-8107de4f6b2a&portalName=HR&personId=8393d114-f109-45ea-9fcc-ad63f1233264').then(response => {
//       setData(response.data)
//       console.log(response.data, 'profile data');
      
//     })
//   }, [])
  //assignement
  //API
  const [assignmentdata, setassignmentData] = useState([])
  useEffect(() => {
    axios.get(`https://webapidev.aitalkx.com/chr/hrdirect/Assignment?personId=8393d114-f109-45ea-9fcc-ad63f1233264&userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR
      ` ).then(response => {
        setassignmentData(response.data)
     // console.log(response.data, 'Assignment data');
      //localStorage.setItem("employee-assignment", JSON.stringify(response.data));
    })
  }, [])
  console.log(assignmentdata, 'data');

  // ** Hooks
  const {
    reset: accountReset,
    control: accountControl,
    handleSubmit: handleAccountSubmit,
    formState: { errors: accountErrors }
  } = useForm({
    defaultValues: defaultAccountValues,
    resolver: yupResolver(accountSchema)
  })


  const {
    reset: personalReset,
    control: personalControl,
    handleSubmit: handlePersonalSubmit,
    formState: { errors: personalErrors }
  } = useForm({
    defaultValues: defaultPersonalValues,
    resolver: yupResolver(personalSchema)
  })


  const {
    reset: socialReset,
    control: socialControl,
    handleSubmit: handleSocialSubmit,
    formState: { errors: socialErrors }
  } = useForm({
    defaultValues: defaultSocialValues,
    resolver: yupResolver(socialSchema)
  })

  // Handle Stepper
  const handleBack = () => {
    setActiveStep(prevActiveStep => prevActiveStep - 1)
  }

  const handleReset = () => {
    setActiveStep(0)
    socialReset({ google: '', twitter: '', facebook: '', linkedIn: '' })
    accountReset({ email: '', username: '', password: '', 'confirm-password': '' })
    personalReset({ country: '', language: [], 'last-name': '', 'first-name': '' })
  }

  const onSubmit = () => {
    setActiveStep(activeStep + 1)
    if (activeStep === steps.length - 1) {
      toast.success('Form Submitted')
    }
  }

  // Handle Password
  const handleClickShowPassword = () => {
    setState({ ...state, showPassword: !state.showPassword })
  }

  const handleMouseDownPassword = event => {
    event.preventDefault()
  }

  // Handle Confirm Password
  const handleClickShowConfirmPassword = () => {
    setState({ ...state, showPassword2: !state.showPassword2 })
  }

  const handleMouseDownConfirmPassword = event => {
    event.preventDefault()
  }



//   console.log(data, 'data');

  const getStepContent = step => {
    switch (step) {
      case 0:
        return (
          <form key={0} onSubmit={handlePersonalSubmit(onSubmit)}>
            <Grid container spacing={5}>
              <Grid item xs={12}>
                <h3>Basic info</h3>
                <hr />
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>Person No : </span>
              </Grid><Grid item xs={12} sm={6}>
                <div>
                  <span>Person full Name :<b>{data.PersonFullName}</b>
                  
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Person email:<b> {data.Email}</b></span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
              </Grid>
              <Grid item xs={10} sm={6}>
                <div>
                  <h3>Personal Details  :</h3>
                </div>
              </Grid>
              <Grid item xs={2} sm={6}>
                <Button size='large' type='submit' variant='contained'>
                  Edit
                </Button>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Title :<b>{data.Title}</b></span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Gender :<b>{data.Gender}</b>
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Person Full Name :<b>{data.PersonFullName}</b>
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span> Marital Status :{data.MaritalStatus}</span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Nationality Name :{data.NationalityName}</span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Religion :{data.Religion}</span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>
                    Date Of Birth:{data.DateOfBirth}
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Personal Email : <b>{data.Email}</b>
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Contact Country Name :{data.NationalityName}
                  </span>
                  
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Mobile :</span>
                  <b> {data.Mobile}</b>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <h3>Present Country Address:{data.NationalityName}
                  </h3>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Unit Number : <b> {data.id}</b> </span>
                  {data.Status}
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Building Number :</span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Street Name :</span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>City :</span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Postal Code : </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Additional Number : <b> {data.Mobile}</b>
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Country Name :{data.NationalityName}
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <h3>Home Country Address:{data.NationalityName}
                  </h3>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Postal Code :
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Unit Number :
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Building Number :
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Street Name : </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>City :
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Postal Code :
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Additional Number : <b> {data.Mobile}</b>
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Country Name :{data.NationalityName}
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <h3>Emergency Contact Info 1
                  </h3>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Emergency Contact Country Name: 1 
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Emergency Contact No : <b> {data.Mobile}</b>
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Emergency Contact Name: 1 
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Relationship:<b> 1</b> 
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <h3>Emergency Contact Info 
                  </h3>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Emergency Contact Name :
                  {data.PersonFullName}
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Emergency Contact Country Name: {data.NationalityName}
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Emergency Contact No:  <b> {data.Mobile}</b>
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}>
                <div>
                  <span>Relationship: 1 
                  </span>
                </div>
              </Grid>
              <Grid item xs={12} sm={6}> </Grid>
              <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                <Button size='large' variant='outlined' color='secondary' onClick={handleBack}>
                  Back
                </Button>
                <Button size='large' type='submit' variant='contained'>
                  Next
                </Button>
              </Grid>
            </Grid>
          </form>
        )
// Assignment pages
      case 1:
        return (
          <form key={1} onSubmit={handlePersonalSubmit(onSubmit)}>
            <Grid container spacing={5}>
              <Grid item xs={12}  sm={6}>
                <h3>Assignment:{data.DepartmentName}</h3>
                
              </Grid>
              <Grid item xs={12}  sm={6}>
              <h3>Assignment:{data.DepartmentName}</h3>
              
            </Grid>
             
           
              <Grid item xs={12} sm={6}>
                <span>Department :{data.DepartmentName}</span>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>Assignment Grade :<b>{data.Status}</b></span>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>Job Id: {data.id} </span>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>Assignment Type :{data.IsPrimaryAssignment}
                </span>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>Position : {data.PositionId}</span>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>Assignment Status :<b>{data.PersonStatus}</b></span>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>Location :</span>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>Probation Period : </span>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>Notice Period :</span>
              </Grid>
              <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                <Button size='large' variant='outlined' color='secondary' onClick={handleBack}>
                  Back
                </Button>
                <Button size='large' type='submit' variant='contained'>
                  Next
                </Button>
              </Grid>
            </Grid>
          </form>
        )
        // Contract pages
      case 2:
        return (

          <form key={2} onSubmit={handleSocialSubmit(onSubmit)}>
            <Grid container spacing={5}>
              <Grid item xs={6}>
                <h3>Contract</h3>
              </Grid>
              <Grid item xs={6} sm={6}>
                <Button size='large'  >
                  <Buttonm />
                </Button>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>ContractType :{data.ContractType}</span>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>Sponsor :{data.SponsorLogo}</span>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>AnnualLeaveEntitlement :{data.AnnualLeaveEntitlement} </span>
              </Grid>
              <Grid item xs={12} sm={6}>
                <span>ContractRenewable :</span>
                {data.ContractRenewable}
              </Grid>
              <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                <Button size='large' variant='outlined' color='secondary' onClick={handleBack}>
                  Back
                </Button>
                <Button size='large' type='submit' variant='contained'>
                  Submit
                </Button>
              </Grid>
            </Grid>
          </form>
        )
        // Leave pages
      case 3:
        return (
          <form key={2} onSubmit={handleSocialSubmit(onSubmit)}>
            <Grid container spacing={5} xs={12}>
              <Grid item xs={12} sm={3}>
                <span>Yearly Entiement:</span><br />
                <span>Leave Balance:</span><br />
                <span> Annual Leave Balance Probation:</span><br />
                <h3>Leave Transaction Details</h3>
              </Grid>
              <Grid item xs={12} sm={8}>
                <span ></span><br />
                <span ></span><br />
                <span >ClickView</span>
                <span></span>
              </Grid>
              <Grid item xs={12} >
                <Mytable />
              </Grid>
              <Grid item xs={12} sm={6}>
              </Grid>
              <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                <Button size='large' variant='outlined' color='secondary' onClick={handleBack}>
                  Back
                </Button>
                <Button size='large' type='submit' variant='contained'>
                  Submit
                </Button>
              </Grid>
            </Grid>
          </form>
        )
        // Attendece pages
      case 4:
        return (
          <form key={2} onSubmit={handleSocialSubmit(onSubmit)}>
          <Grid container spacing={5} xs={12}>
          <Grid item xs={12} sm={3}>
          <Card sx={{p :3}}>
          <TextField
            fullWidth
            id='date'
            label='Month'
            type='date'
            defaultValue='YYYY-MM-DD'
            InputLabelProps={{
              shrink: true
            }}
          />

         
        </Card>
          </Grid>

          <Grid item xs={12} sm={3}>
          <Card sx={{p :3}}>
          <TextField
            fullWidth
            id='date'
            label='PeriodFrom'
            type='date'
            defaultValue='YYYY-MM-DD'
            InputLabelProps={{
              shrink: true
            }}
          />

         
        </Card>
          </Grid>
          <Grid item xs={12} sm={3}>
          <Card sx={{p :3}}>
          <TextField
            fullWidth
            id='date'
            label='To'
            type='date'
            defaultValue='YYYY-MM-DD'
            InputLabelProps={{
              shrink: true
            }}
          />

         
        </Card>
          </Grid>
          <Grid item xs={12} sm={3}>
  
        
          <Button  type='Search' variant='contained'>
          Search
        </Button>
         
      
          </Grid>
        
         
        
          <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'space-between' }}>
            <Button size='large' variant='outlined' color='secondary' onClick={handleBack}>
              Back
            </Button>
            <Button size='large' type='submit' variant='contained'>
              Submit
            </Button>
          </Grid>
        </Grid>
          </form>
        ) 

        // Document pages
      case 5:
        return (
          <form key={2} onSubmit={handleSocialSubmit(onSubmit)}>
            <Grid container spacing={5}>
              <Grid item xs={12} >
                <Button variant='contained'>Upload Document</Button><br />
              </Grid>
              <Grid item xs={12}>
                <Mytable />
              </Grid>
              <Grid item xs={12} sm={6}>
              </Grid>
              <Grid item xs={12} sm={6}>
              </Grid>
              <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                <Button size='large' variant='outlined' color='secondary' onClick={handleBack}>
                  Back
                </Button>
                <Button size='large' type='submit' variant='contained'>
                  Submit
                </Button>
              </Grid>
            </Grid>
          </form>
        )
        // Depertment pages
      case 6:
        return (
          <form key={2} onSubmit={handleSocialSubmit(onSubmit)}>
            <Grid container spacing={5}>
              <Grid item xs={12}>
                <h3>Dependent</h3>
              </Grid>
              <Grid item xs={12}>
                <Mytable />
              </Grid>
              <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                <Button size='large' variant='outlined' color='secondary' onClick={handleBack}>
                  Back
                </Button>
                <Button size='large' type='submit' variant='contained'>
                  Submit
                </Button>
              </Grid>
            </Grid>
          </form>
        )
        // Payroll pages
      case 7:
        return (
          <form key={2} onSubmit={handleSocialSubmit(onSubmit)}>
            <Grid container spacing={5}>
              <Rayrolll />
              <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                <Button size='large' variant='outlined' color='secondary' onClick={handleBack}>
                  Back
                </Button>
                <Button size='large' type='submit' variant='contained'>
                  Submit
                </Button>
              </Grid>
            </Grid>
          </form>
        )
        // Payslip pages
      case 8:
        return (
          <form key={2} onSubmit={handleSocialSubmit(onSubmit)}>
            <Grid container spacing={5}>
              <Rayslip />
              <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                <Button size='large' variant='outlined' color='secondary' onClick={handleBack}>
                  Back
                </Button>
                <Button size='large' type='submit' variant='contained'>
                  Submit
                </Button>
              </Grid>
            </Grid>
          </form>
        )
      default:
        return null
    }
  }
  const renderContent = () => {
    if (activeStep === steps.length) {
      return (
        <Fragment>
          <Typography>All steps are completed!</Typography>
          <Box sx={{ mt: 4, display: 'flex', justifyContent: 'flex-end' }}>
            <Button size='large' variant='contained' onClick={handleReset}>
              Reset
            </Button>
          </Box>
        </Fragment>
      )
    } else {
      return getStepContent(activeStep)
    }
  }
  return (
    <Card>
      <CardContent>
        <StepperWrapper>
          <Stepper activeStep={activeStep}>
            {steps.map((step, index) => {
              const labelProps = {}
              // if (index === activeStep) {
              //   labelProps.error = false
              //   if (
              //     (accountErrors.email ||
              //       accountErrors.username ||
              //       accountErrors.password ||
              //       accountErrors['confirm-password']) &&
              //     activeStep === 0
              //   ) {
              //     labelProps.error = true
              //   } else if (
              //     (personalErrors.country ||
              //       personalErrors.language ||
              //       personalErrors['last-name'] ||
              //       personalErrors['first-name']) &&
              //     activeStep === 1
              //   ) {
              //     labelProps.error = true
              //   } else if (
              //     (socialErrors.google || socialErrors.twitter || socialErrors.facebook || socialErrors.linkedIn) &&
              //     activeStep === 2
              //   ) {
              //     labelProps.error = true
              //   } else {
              //     labelProps.error = false
              //   }
              // }

              return (
                <Step key={index}>
                  {/* <StepLabel {...labelProps} StepIconComponent={StepperCustomDot}>
                    <div className='step-label'>
                      <Typography className='step-number'>{`0${index + 1}`}</Typography>
                      <div>
                        <Typography className='step-title'>{step.title}</Typography>
                        <Typography className='step-subtitle'>{step.subtitle}</Typography>
                      </div>
                    </div>
                  </StepLabel> */}
                </Step>
              )
            })}
          </Stepper>
        </StepperWrapper>
      </CardContent>
      <Divider sx={{ m: '0 !important' }} />
      <CardContent>{renderContent()}</CardContent>
    </Card>
  )
}
export default StepperLinearWithValidation