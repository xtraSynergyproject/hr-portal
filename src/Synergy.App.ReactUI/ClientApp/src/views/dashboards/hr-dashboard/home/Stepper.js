import * as React from 'react';
import { useTheme } from '@mui/material/styles';
import Box from '@mui/material/Box';
import MobileStepper from '@mui/material/MobileStepper';
import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import KeyboardArrowLeft from '@mui/icons-material/KeyboardArrowLeft';
import KeyboardArrowRight from '@mui/icons-material/KeyboardArrowRight';
import SwipeableViews from 'react-swipeable-views';
import { autoPlay } from 'react-swipeable-views-utils';
import { deepPurple } from '@mui/material/colors';

const AutoPlaySwipeableViews = autoPlay(SwipeableViews);

const images = [
  {
    label: 'Create Post',
    imgPath:
      'https://files.spazioweb.it/44/db/44db50d1-f58c-4cff-b619-6af24d31af21.jpg',
  },
  {
    label: 'Create Post',
    imgPath:
      'http://st.depositphotos.com/1325771/2521/i/450/depositphotos_25218381-Group-portrait-of-a-professional-business-team-looking-confidently-at-camera.jpg',
  },
  {
    label: 'Create Post',
    imgPath:
      'https://t4.ftcdn.net/jpg/03/72/48/83/360_F_372488329_6bJkPEgd3Ectqt3cv23xrSJpNwhBzaov.jpg',
  },
  {
    label: 'Create Post',
    imgPath:
      'https://snacknation.com/wp-content/uploads/2022/03/CoAdvantage-e1646179929156.png',
  },
];

function Stepper() {
  const theme = useTheme();
  const [activeStep, setActiveStep] = React.useState(0);
  const maxSteps = images.length;

  const handleNext = () => {
    setActiveStep((prevActiveStep) => prevActiveStep + 1);
  };

  const handleBack = () => {
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
  };

  const handleStepChange = (step) => {
    setActiveStep(step);
  };

  return (
    <Box sx={{ maxWidth: 1050, flexGrow: 1,borderRadius: 5, margin:'auto' }}>
      <Paper
        square
        elevation={0}
        sx={{
          display: 'flex',
          alignItems: 'center',
          height: 50,
          pl: 2,
          backgroundImage: 'linear-gradient(98deg, #C6A7FE, #9155FD 94%)'
        
         
        }}
      >
        <Typography sx={{color: 'white'}}>{images[activeStep].label}</Typography>
      </Paper>
      <AutoPlaySwipeableViews sx={{width: 900}}
        axis={theme.direction === 'rtl' ? 'x-reverse' : 'x'}
        index={activeStep}
        onChangeIndex={handleStepChange}
        enableMouseEvents
      >
        {images.map((step, index) => (
          <div key={step.label}>
            {Math.abs(activeStep - index) <= 2 ? (
              <Box
                component="img"
                sx={{
                  height: 400,
                  display: 'block',
                  maxWidth: 1200,
                  overflow: 'hidden',
                  width: '100%',
                  padding: 'auto',
                  borderBottomRightRadius:80, 
                  
                 
                }}
                src={step.imgPath}
                alt={step.label}
              />
            ) : null}
          </div>
        ))}
      </AutoPlaySwipeableViews>
      <MobileStepper sx={{ borderTopRightRadius:40, borderBottomRightRadius:40,}}
        steps={maxSteps}
        position="static"
        activeStep={activeStep}
        nextButton={
          <Button
            size="small"
            onClick={handleNext}
            disabled={activeStep === maxSteps - 1}
          >
            Next
            {theme.direction === 'rtl' ? (
              <KeyboardArrowLeft />
            ) : (
              <KeyboardArrowRight />
            )}
          </Button>
        }
        backButton={
          <Button size="small" onClick={handleBack} disabled={activeStep === 0}>
            {theme.direction === 'rtl' ? (
              <KeyboardArrowRight />
            ) : (
              <KeyboardArrowLeft />
            )}
            Back
          </Button>
        }
      />
    </Box>
  );
}

export default Stepper;
