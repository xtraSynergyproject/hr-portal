// ** MUI Imports
import Card from '@mui/material/Card'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'

const CardInfluencer = () => {
  return (
    <Card sx={{ width: '100%', height: '20rem', border: '1px solid' }}>
      <CardContent
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          backgroundColor: 'lightgrey',
          fontWeight: 400,

          height: '40px',
          width: '100%'
        }}
      >
        <Typography sx={{ color: 'black'}}> Requests Summary
        <input></input>
        </Typography>
      </CardContent>
    </Card>
  )
}

export default CardInfluencer
