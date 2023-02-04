import * as React from 'react';
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';

export default function BasicButtons() {
  return (
    <Stack spacing={5} direction="row" paddingLeft={31}>
      <Button variant="contained">PersonFullName</Button>
      <Button variant="contained">SponsorshipNo</Button>
      <Button variant="contained">BiometricId</Button>
      <Button variant="contained">PunchingTime</Button>
      <Button variant="contained">DevicePunchingTy</Button>
      <Button variant="contained">DeviceName</Button>
    </Stack>
    
    
  );
  
}