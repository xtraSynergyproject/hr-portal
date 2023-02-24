import * as React from 'react';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';

export default function SelectSmall() {
  const [age, setAge] = React.useState('');

  const handleChange = (event) => {
    setAge(event.target.value);
  };

  return (
    <FormControl sx={{ m: 1, minWidth: 120 }} size="small">
      <InputLabel id="demo-select-small">GrantStatus</InputLabel>
      <Select
      sx={{ width: '400px',height:'55px' }}
        labelId="demo-select-small"
        id="demo-select-small"
        value={age}
        label="GrantStatus"
        onChange={handleChange}
      >
   
        <MenuItem value={10}>Grant</MenuItem>
        <MenuItem value={20}>Revoked</MenuItem>
      </Select>
    </FormControl>
  );
}
