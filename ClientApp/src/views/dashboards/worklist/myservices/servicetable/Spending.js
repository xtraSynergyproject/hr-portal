// ** MUI Imports
import Card from '@mui/material/Card'
import Button from '@mui/material/Button'
// import Typography from '@mui/material/Typography'
// import CardHeader from '@mui/material/CardHeader'
import CardContent from '@mui/material/CardContent'
// import CardActions from '@mui/material/CardActions'
import Paper from '@mui/material/Paper'
import Table from '@mui/material/Table'
import TableRow from '@mui/material/TableRow'
import TableHead from '@mui/material/TableHead'
import TableBody from '@mui/material/TableBody'
import TableCell from '@mui/material/TableCell'
import TableContainer from '@mui/material/TableContainer'

const createData = (name, calories, fat, carbs, protein) => {
    return { name, calories, fat, carbs, protein }
  }
  
  const rows = [
    createData('Frozen yoghurt', 159, 6.0, 24, 4.0),
    createData('Ice cream sandwich', 237, 9.0, 37, 4.3),
    createData('Eclair', 262, 16.0, 24, 6.0),
    createData('Cupcake', 305, 3.7, 67, 4.3),
    createData('Gingerbread', 356, 16.0, 49, 3.9)
  ]
  

const CompletedTable = () => {
  return (
    <Card sx={{mt:28}}>
      
      <CardContent>
      <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} aria-label='simple table'>
        <TableHead>
          <TableRow>
            <TableCell>Service No</TableCell>
            <TableCell align='right'>Service</TableCell>
            <TableCell align='right'>Service Status</TableCell>
            <TableCell align='right'>Task No</TableCell>
            <TableCell align='right'>Task Subject</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {rows.map(row => (
            <TableRow
              key={row.name}
              sx={{
                '&:last-of-type td, &:last-of-type th': {
                  border: 0
                }
              }}
            >
              <TableCell component='th' scope='row'>
                {row.name}
              </TableCell>
              <TableCell align='right'>{row.calories}</TableCell>
              <TableCell align='right'>{row.fat}</TableCell>
              <TableCell align='right'>{row.carbs}</TableCell>
              <TableCell align='right'>{row.protein}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
      </CardContent>
   </Card>
  )
}

export default CompletedTable
