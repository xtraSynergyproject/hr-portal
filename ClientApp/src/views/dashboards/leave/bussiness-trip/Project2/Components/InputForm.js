import React from 'react'
// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Step from '@mui/material/Step'
import Button from '@mui/material/Button'
// import Stepper from '@mui/material/Stepper'
import StepLabel from '@mui/material/StepLabel'
import CardHeader from '@mui/material/CardHeader'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import StepContent from '@mui/material/StepContent'

// ** Third Party Imports
// import clsx from 'clsx'
import toast from 'react-hot-toast'

// ** Custom Components Imports
// import StepperCustomDot from './StepperCustomDot'
import StepperCustomDot from 'src/views/forms/form-wizard/StepperCustomDot'

// ** Styled Component
import StepperWrapper from 'src/@core/styles/mui/stepper'
import PersonalInfoForm from './FForm.js/PersonalInfoForm'
import AddressInfoForm from './FForm.js/AddressInfoForm'
import ContactInfoForm from './FForm.js/ContactInfoForm'
import EmergencyContact from './FForm.js/EmergencyContact'


const steps = [{a:1},{a:1},{a:1},{a:1}]


function InputForm() {

     // ** States
  const [activeStep, setActiveStep] = useState(0)

  // Handle Stepper
  const handleBack = () => {
    setActiveStep(prevActiveStep => prevActiveStep - 1)
  }

  const handleNext = () => {
    setActiveStep(prevActiveStep => prevActiveStep + 1)
    if (activeStep === steps.length - 1) {
      toast.success('Completed All Steps!!')
    }
  }

  const handleReset = () => {
    setActiveStep(0)
  }

  return (
    <Card>
      <CardHeader title='Add Employee' />
      <CardContent>
        <StepperWrapper>
                <Step>
                  <StepLabel 
                  StepIconComponent={StepperCustomDot}
                  >
                    <div className='step-label' >
                      <Typography className='step-number'>01</Typography>
                      <div>
                        <Typography className='step-title'>Personal Information</Typography>
                        <Typography className='step-subtitle'>Add Personal info Of Employee</Typography>
                      </div>
                    </div>
                  </StepLabel>
                  <StepContent>
                    <PersonalInfoForm/>
                  
                  </StepContent>
                </Step>
              

                <Step>
                  <StepLabel 
                  StepIconComponent={StepperCustomDot}
                  >
                    <div className='step-label'>
                      <Typography className='step-number'>02</Typography>
                      <div>
                        <Typography className='step-title'>Address Information</Typography>
                        <Typography className='step-subtitle'>Add Address Information</Typography>
                      </div>
                    </div>
                  </StepLabel>
                  <StepContent>
                    <AddressInfoForm/>
                   
                  </StepContent>
                </Step>
              

                <Step>
                  <StepLabel 
                  StepIconComponent={StepperCustomDot}
                  >
                    <div className='step-label'>
                      <Typography className='step-number'>03</Typography>
                      <div>
                        <Typography className='step-title'>Contact Information</Typography>
                        <Typography className='step-subtitle'>Add Contact Information</Typography>
                      </div>
                    </div>
                  </StepLabel>
                  <StepContent>
                    <ContactInfoForm/>
                   
                  </StepContent>
                </Step>
              

                <Step>
                  <StepLabel 
                  StepIconComponent={StepperCustomDot}
                  >
                    <div className='step-label'>
                      <Typography className='step-number'>04</Typography>
                      <div>
                        <Typography className='step-title'>Emergency Contact Information</Typography>
                        <Typography className='step-subtitle'>Add Eemergency Contacts</Typography>
                      </div>
                    </div>
                  </StepLabel>
                  <StepContent>
                    <EmergencyContact/>
                   
                  </StepContent>
                </Step>

                <div className='button-wrapper'>
                      <Button
                        size='small'
                        color='secondary'
                        variant='outlined'
                        onClick={handleBack}
                        disabled={activeStep === 0}
                      >
                        Back
                      </Button>
                      <Button size='small' variant='contained' onClick={handleNext} sx={{ ml: 4 }}>
                        {activeStep === steps.length - 1 ? 'Finish' : 'Next'}
                      </Button>
                    </div> 
              



        </StepperWrapper>
        {activeStep === steps.length && (
          <Box sx={{ mt: 2 }}>
            <Typography>All steps are completed!</Typography>
            <Button size='small' sx={{ mt: 2 }} variant='contained' onClick={handleReset}>
              Reset
            </Button>
          </Box>
        )}
      </CardContent>
    </Card>
  )
}

export default InputForm